using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Segment : MonoBehaviour {

    AssetManager assetManager;
    protected Robot robot;

    public Material defaultMaterial;
    public Renderer[] renderers;
    public Part parent;
    public List<Segment> connectedSegments;
    public List<Segment> touchingSegments;
    public Collider collider;
    public Collider trigger;
    public Rigidbody rb;

    protected bool active;

    public void connect()
    {
        foreach (Segment touchingSegment in touchingSegments)
        {
            if (!touchingSegment.isConnected(this) && !touchingSegment.parent.unconnectable)
            {
                FixedJoint fJoint = gameObject.AddComponent<FixedJoint>();
                fJoint.connectedBody = touchingSegment.rb;
                connectedSegments.Add(touchingSegment);
                touchingSegment.connectedSegments.Add(this);
                if (!touchingSegment.parent.placed)
                {
                    touchingSegment.parent.place();
                }
            }
        }

        rb.useGravity = false;
        resetPhysics();
        active = false;
        refresh();
    }

	// Use this for initialization
	void Awake () {
        assetManager = AssetManager.Instance;
        connectedSegments = new List<Segment>();
        touchingSegments = new List<Segment>();
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

    public bool isConnected(Segment other)
    {
        foreach (Segment connectedSegment in connectedSegments) 
        {
            if (other.Equals(connectedSegment))
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

    void updateTouchingSegments(Segment segment)
    {
        if (segment != null)
        {
            bool listed = false;
            foreach (Segment touchingSegment in touchingSegments)
            {
                if (touchingSegment.Equals(segment))
                {
                    listed = true;
                }
            }
            if (!listed)
            {
                touchingSegments.Add(segment);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!parent.placed && other.gameObject.tag == Constants.LAYER_COMPONENT)
        {
            Segment segment = other.GetComponent<Segment>();
            if (segment == null)
            {
                segment = other.GetComponentInParent<Segment>();
            }
            updateTouchingSegments(segment);
            parent.evaluateState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        touchingSegments = new List<Segment>();
        if (!parent.placed)
        {
            parent.evaluateState();
        }
    }

    protected void resetPhysics()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected abstract void init();
    protected abstract void update();
    protected abstract void refresh();
}
