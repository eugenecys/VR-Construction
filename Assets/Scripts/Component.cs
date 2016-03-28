using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Component : MonoBehaviour {

    AssetManager assetManager;
    protected Robot robot;

    public Material defaultMaterial;
    public Renderer[] renderers;
    public Part parent;
    public List<Component> connectedComponents;
    public List<Component> touchingComponents;
    public bool movable { get; protected set; }
    public Collider collider;
    public Collider trigger;
    public Rigidbody rb;

    protected bool active;
    
	// Use this for initialization
	void Awake () {
        assetManager = AssetManager.Instance;
        connectedComponents = new List<Component>();
        touchingComponents = new List<Component>();
        robot = Robot.Instance;
	}

    void Start()
    {
        active = false;
        parent.evaluateState();
        init();
    }

    public void deploy()
    {
        rb.useGravity = true;
        resetPhysics(); 
        active = true;
        refresh();
    }

    public void activate()
    {
        rb.useGravity = false;
        resetPhysics(); 
        active = false;
        refresh();
    }

    public void reset()
    {
        rb.useGravity = false;
        resetPhysics(); 
        active = false;
        refresh();
    }

    public void connect()
    {
        foreach (Component touchingComponent in touchingComponents)
        {
            if (!touchingComponent.isConnected(this) && !touchingComponent.parent.unconnectable)
            {
                FixedJoint fJoint = gameObject.AddComponent<FixedJoint>();
                fJoint.connectedBody = touchingComponent.rb;
                connectedComponents.Add(touchingComponent);
                touchingComponent.connectedComponents.Add(this);
                if (!touchingComponent.parent.placed)
                {
                    touchingComponent.parent.place();
                }
            }
        }

        rb.useGravity = false;
        resetPhysics();
        active = false;
        refresh();
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

    public void setMaterial(Material _material)
    {
        foreach (Renderer rend in renderers)
        {
            rend.material = _material;
        }
    }

    public void setDefaultMaterial()
    {
        foreach (Renderer rend in renderers)
        {
            rend.material = defaultMaterial;
        }
    }

    void Update()
    {
        update();
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
        if (!parent.placed && other.gameObject.tag == Constants.LAYER_COMPONENT)
        {
            Component component = other.GetComponent<Component>();
            if (component == null)
            {
                component = other.GetComponentInParent<Component>();
            }
            updateTouchingComponents(component);
            parent.evaluateState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        touchingComponents = new List<Component>();
        if (!parent.placed)
        {
            parent.evaluateState();
        }
    }

    void resetPhysics()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected abstract void init();
    protected abstract void update();
    protected abstract void refresh();
}
