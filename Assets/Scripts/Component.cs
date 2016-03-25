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
        setState(State.Unconnectable);
        init();
	}

    public bool connectable()
    {
        return otherCollider != null;
    }

    public void connect()
    {
        
    }

    private void setState(State state)
    {
        switch (state)
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

    void OnTriggerEnter(Collider other)
    {
        
        otherCollider = other;
        setState(State.Connectable);
    }

    void OnTriggerExit(Collider other)
    {
        otherCollider = null;
        setState(State.Unconnectable);
    }

    protected abstract void update();
    protected abstract void init();

}
