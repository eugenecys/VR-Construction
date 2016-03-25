using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Component : MonoBehaviour {

    AssetManager assetManager;

    public enum State
    {
        Deployed,
        Connectable,
        Unconnectable
    }

    public Material defaultMaterial;
    public State state;
    public Renderer[] renderers;
    public GameObject parent;
    List<Component> connectedComponents;
    List<Component> touchingComponents;
    public bool movable { get; protected set; }
    public Collider collider;
    public Collider trigger;
    public Rigidbody rb;

    public bool deployed
    {
        get
        {
            return state.Equals(State.Deployed);
        }
    }

    public bool connectable
    {
        get 
        {
            return state.Equals(State.Connectable);
        }
    }

	// Use this for initialization
	void Awake () {
        assetManager = AssetManager.Instance;
        connectedComponents = new List<Component>();
        touchingComponents = new List<Component>();
	}

    void Start()
    {
        setState(State.Unconnectable);
    }

    public void deploy()
    {
        rb.useGravity = true;
        setState(State.Deployed);
    }

    public void connect()
    {
        if (connectable)
        {
            foreach (Component touchingComponent in touchingComponents)
            {
                if (!touchingComponent.isConnected(this))
                {
                    FixedJoint fJoint = gameObject.AddComponent<FixedJoint>();
                    fJoint.connectedBody = touchingComponent.rb;
                    connectedComponents.Add(touchingComponent);
                    touchingComponent.connectedComponents.Add(this);
                    if (!touchingComponent.deployed)
                    {
                        touchingComponent.connect();
                    }
                }
            }
            deploy();

            if (parent != null)
            {
                Component[] allComponents = parent.GetComponentsInChildren<Component>();
                foreach (Component component in allComponents)
                {
                    if (!component.deployed)
                    {
                        component.connect();
                        component.deploy();
                    }
                }
            }
            Component[] childComponents = GetComponentsInChildren<Component>();
            foreach (Component component in childComponents)
            {
                if (!component.deployed)
                {
                    component.connect();
                    component.deploy();
                }
            }
        }
    }

    public bool isConnected(Component other)
    {
        foreach (Component connectedComponent in connectedComponents) 
        {
            if (other.Equals(connectedComponent))
            {
                return true;
            }
        }
        return false;
    }

    private void setState(State newState)
    {
        switch (newState)
        {
            case State.Deployed:
                state = State.Deployed;
                foreach (Renderer rend in renderers)
                {
                    rend.material = defaultMaterial;
                }
                break;
            case State.Connectable:
                state = State.Connectable; 
                foreach (Renderer rend in renderers)
                {
                    rend.material = assetManager.connectableMaterial;
                }
                break;
            case State.Unconnectable:
                state = State.Unconnectable;
                foreach (Renderer rend in renderers)
                {
                    rend.material = assetManager.unconnectableMaterial;
                }
                break;
        }
    }

    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (!state.Equals(State.Deployed) && other.gameObject.tag == Constants.LAYER_COMPONENT)
        {
            Component component = other.GetComponent<Component>();
            if (component == null)
            {
                component = other.GetComponentInParent<Component>();
            } 
            
            if (component != null) 
            {
                bool listed = false;
                foreach (Component touchingComponent in touchingComponents)
                {
                    if (touchingComponent.Equals(component))
                    {
                        listed = true;
                    }
                }
                if (!listed)
                {
                    touchingComponents.Add(component);
                }
                setState(State.Connectable);
                Debug.Log(touchingComponents.Count);
            } 
        }
    }

    void OnTriggerExit(Collider other)
    {
        touchingComponents = new List<Component>();
        if (!state.Equals(State.Deployed))
        {
            setState(State.Unconnectable);
        }
    }

}
