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
    public Renderer rend;
    List<Component> connectedComponents;
    public bool movable { get; protected set; }
    public Collider collider;
    public Collider trigger;
    public Rigidbody rb;
    
    private Component otherComponent;

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
        init();
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
            FixedJoint fJoint = gameObject.AddComponent<FixedJoint>();
            fJoint.connectedBody = otherComponent.rb;
            deploy();
        }
    }

    public bool isConnected(Component other)
    {
        foreach (Component connectedComponnet in connectedComponents) 
        {
            if (this.Equals(connectedComponnet))
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
                rend.material = defaultMaterial;
                break;
            case State.Connectable:
                state = State.Connectable;
                rend.material = assetManager.connectableMaterial;
                break;
            case State.Unconnectable:
                state = State.Unconnectable;
                rend.material = assetManager.unconnectableMaterial;
                break;
        }
    }

    void Update()
    {
        update();
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
                otherComponent = component;
                setState(State.Connectable);
            } 
        }
    }

    void OnTriggerExit(Collider other)
    {
        otherComponent = null;
        if (!state.Equals(State.Deployed))
        {
            setState(State.Unconnectable);
        }
    }

    protected abstract void update();
    protected abstract void init();

}
