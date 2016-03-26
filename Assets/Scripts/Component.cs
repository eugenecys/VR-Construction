using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Component : MonoBehaviour {

    AssetManager assetManager;

    public enum State
    {
        Deployed,
        Connectable,
        Unconnectable,
        Free
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

    public bool free
    {
        get
        {
            return state.Equals(State.Free);
        }
    }

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
        setState(State.Free);
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
            connectTouchingComponents();
            deploy();
            propagateConnection();
        }
    }

    void propagateConnection()
    {
        if (parent == null)
        {
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
        else
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
    }

    void connectTouchingComponents()
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
            case State.Free:
                state = State.Free;
                foreach (Renderer rend in renderers)
                {
                    rend.material = assetManager.freeMaterial;
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
            }

            if (touchingComponents.Count > 0)
            {
                if (!hasComponentOverlap())
                {
                    setState(State.Connectable);
                }
                else
                {
                    setState(State.Unconnectable);
                }
            }
            else
            {
                setState(State.Free);
            }
        }
    }

    bool hasComponentOverlap()
    {
        bool overlap = false;
        Component[] allComponents;
        if (parent == null)
        {
            allComponents = GetComponentsInChildren<Component>();
        }
        else 
        {
            allComponents = parent.GetComponentsInChildren<Component>();
        }
        if (allComponents != null && allComponents.Length > 1)
        {
            for (int i = 0; i < allComponents.Length - 1; i++)
            {
                for (int j = 1; j < allComponents.Length; j++)
                {
                    if (i != j)
                    {
                        if (hasTouchingComponentOverlap(allComponents[i], allComponents[j]))
                        {
                            overlap = true;
                        }
                    }
                }
            }
        } 
        return overlap;
    }

    bool hasTouchingComponentOverlap(Component a, Component b) 
    {
        foreach (Component aC in a.touchingComponents)
        {
            foreach (Component bC in b.touchingComponents) 
            {
                if (aC.Equals(bC))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnTriggerExit(Collider other)
    {
        touchingComponents = new List<Component>();
        if (!state.Equals(State.Deployed))
        {
            setState(State.Free);
        }
    }

}
