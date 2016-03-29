using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Segment : MonoBehaviour {

    AssetManager assetManager;
    protected Robot robot;
    
    public Part parent;
    public List<Segment> connectedSegments;
    public List<Segment> touchingSegments;
    public Collider collider;
    public Rigidbody rigidbody;
    public Collider trigger;

    protected bool active;

    public void connect()
    {
        foreach (Segment touchingSegment in touchingSegments)
        {
            if (!touchingSegment.isConnected(this) && !touchingSegment.parent.unconnectable)
            {
                FixedJoint fJoint = gameObject.AddComponent<FixedJoint>();
                fJoint.connectedBody = touchingSegment.rigidbody;
                connectedSegments.Add(touchingSegment);
                touchingSegment.connectedSegments.Add(this);
                if (!touchingSegment.parent.placed)
                {
                    touchingSegment.parent.place();
                }
            }
        }

        rigidbody.useGravity = false;
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
        trigger.isTrigger = false;
        rigidbody.useGravity = true;
        resetPhysics(); 
        active = true;
        refresh();
    }

    public void activate()
    {
        trigger.isTrigger = false;
        rigidbody.useGravity = false;
        resetPhysics(); 
        active = true;
        refresh();
    }

    public void reset()
    {
        trigger.isTrigger = true;
        rigidbody.useGravity = false;
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
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    protected abstract void init();
    protected abstract void update();
    protected abstract void refresh();
}
