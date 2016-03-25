using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Component : MonoBehaviour {

    public enum State
    {
        Deployed,
        Connectable,
        Unconnectable
    }

    public State state;

    List<Component> connectedComponents;
    public bool movable { get; protected set; }
    public Collider collider;
    public Collider trigger;
    public Rigidbody rb;


	// Use this for initialization
	void Awake () {
        connectedComponents = new List<Component>();
        setState(State.Unconnectable);
        init();
	}

    public bool connectable()
    {
        return false;
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
                break;
            case State.Connectable:
                state = State.Connectable;
                break;
            case State.Unconnectable:
                state = State.Unconnectable;
                break;
        }
    }

    void Update()
    {
        update();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("COOL");
    }

    protected abstract void update();
    protected abstract void init();

}
