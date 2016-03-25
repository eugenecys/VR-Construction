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
    
    private Collider otherCollider;

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

    public bool connectable()
    {
        return otherCollider != null;
    }

    public void connect()
    {
        
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
            Component otherComponent = other.GetComponent<Component>();
            if (otherComponent == null)
            {
                otherComponent = other.GetComponentInParent<Component>();
            } 
            
            if (otherComponent != null) 
            {
                otherCollider = other;
                setState(State.Connectable);
            } 
        }
    }

    void OnTriggerExit(Collider other)
    {
        otherCollider = null;
        if (!state.Equals(State.Deployed))
        {
            setState(State.Unconnectable);
        }
    }

    protected abstract void update();
    protected abstract void init();

}
