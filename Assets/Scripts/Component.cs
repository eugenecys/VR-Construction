using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Component : MonoBehaviour {

    AssetManager assetManager;

    public enum State
    {
        Conflict,
        Unconnectable,
        Connectable,
        Available,
        Free,
        Inactive,
        Active,
        Deployed
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

    public bool unconnectable
    {
        get
        {
            return state.Equals(State.Unconnectable);
        }
    }

    public bool conflict
    {
        get
        {
            return state.Equals(State.Conflict);
        }
    }

    public bool inactive
    {
        get
        {
            return state.Equals(State.Inactive);
        }
    }

    public bool active
    {
        get
        {
            return state.Equals(State.Active);
        }
    }

    public bool available
    {
        get
        {
            return state.Equals(State.Available);
        }
    }

    public bool placed
    {
        get
        {
            return (state.Equals(State.Active) || state.Equals(State.Deployed) || state.Equals(State.Inactive));
        }
    }

    public bool isAComponent
    {
        get
        {
            return (gameObject.tag == Constants.LAYER_COMPONENT);
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

    public void place()
    {
        rb.useGravity = false;
        setState(State.Inactive);
    }

    public void connect()
    {
        Component[] others = getAllComponents();
        foreach (Component cpt in others)
        {
            if (cpt.connectable)
            {
                connectTouchingComponents();
                deploy();
                propagateConnection();
            }
        }
    }

    void propagateConnection()
    {
        Component[] components = getAllComponents();
        foreach (Component component in components)
        {
            if (!component.placed)
            {
                component.connect();
                component.deploy();
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

    private void evaluateState()
    {
        bool conflict = hasComponentOverlap();
        Component[] others = getAllComponents();
        //Check if conflict
        if (conflict)
        {
            setState(State.Conflict);
            foreach (Component cpt in others)
            {
                if (!cpt.conflict && !cpt.unconnectable)
                {
                    cpt.setState(State.Unconnectable);
                }
            }
        }
        else
        {
            //If no conflict, check if others have conflict
            bool hasConflictElsewhere = false;
            foreach (Component cpt in others)
            {
                if (cpt.conflict)
                {
                    hasConflictElsewhere = true;
                }
            }

            if (hasConflictElsewhere)
            {
                setState(State.Unconnectable);
            }
            else
            {
                //If others have no conflict, check if there's touching components
                if (touchingComponents.Count > 0)
                {
                    setState(State.Connectable);
                    foreach (Component cpt in others)
                    {
                        if (!cpt.connectable && !cpt.available)
                        {
                            cpt.setState(State.Available);
                        }
                    }
                }
                else
                {
                    //If no touching components, check if others have touching components
                    bool hasTouchingComponentsElsewhere = false;
                    foreach (Component cpt in others)
                    {
                        if (cpt.touchingComponents.Count > 0)
                        {
                            hasTouchingComponentsElsewhere = true;
                        }
                    }

                    if (hasTouchingComponentsElsewhere)
                    {
                        setState(State.Available);
                    }
                    else
                    {
                        //If others have no touching components, set free.
                        setState(State.Free);
                    }
                }
            }
        }
    }

    private void setState(State newState)
    {
        switch (newState)
        {
            case State.Inactive:
            case State.Active:
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
            case State.Conflict:
                state = State.Conflict;
                foreach (Renderer rend in renderers)
                {
                    rend.material = assetManager.conflictMaterial;
                }
                break;
            case State.Available:
                state = State.Available;
                foreach (Renderer rend in renderers)
                {
                    rend.material = assetManager.availableMaterial;
                }
                break;
        }
    }

    void Update()
    {

    }

    void updateTouchingComponents(Component component)
    {
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
    }

    void OnTriggerStay(Collider other)
    {
        if (!placed && other.gameObject.tag == Constants.LAYER_COMPONENT)
        {
            Component component = other.GetComponent<Component>();
            if (component == null)
            {
                component = other.GetComponentInParent<Component>();
            }
            updateTouchingComponents(component);
            evaluateState();
        }
    }

    bool hasComponentOverlap()
    {
        bool overlap = false;
        Component[] allComponents = getAllComponents();
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

    Component[] getAllComponents()
    {
        if (parent == null)
        {
            return GetComponentsInChildren<Component>();
        }
        else
        {
            return parent.GetComponentsInChildren<Component>();
        }
    }

}
