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
    public Collider detector;

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
                parent.addConnectedPart(touchingSegment.parent);
                touchingSegment.parent.addConnectedPart(parent);
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
        rigidbody.isKinematic = true;
	}

    void Start()
    {
        active = false;
        parent.evaluateState();
        init();
    }

    public void enablePhysics()
    {
        rigidbody.isKinematic = false;
    }

    public void disablePhysics()
    {
        rigidbody.isKinematic = true;
    }

    public void deploy()
    {
        //trigger.isTrigger = false;
        rigidbody.useGravity = true;
        enablePhysics();
        resetPhysics(); 
        active = true;
        refresh();
    }

    public void activate()
    {
        //trigger.isTrigger = false;
        rigidbody.useGravity = false;
        disablePhysics();
        resetPhysics(); 
        active = true;
        refresh();
    }

    public void reset()
    {
        //trigger.isTrigger = true;
        rigidbody.useGravity = false;
        disablePhysics();
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
        if (segment != null && !segment.parent.template)
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("building"))
        {
            other.gameObject.SendMessage("GiveAttack");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (parent.template && other.GetComponentInParent<Part>())
        {
            return;
        }
        else if (parent.template)
        {
            parent.highlight();
        } 
        else if (!parent.placed && other.gameObject.tag == Constants.LAYER_COMPONENT)
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
        if (parent.template)
        {
            parent.unhighlight();
        }
        else
        {
            parent.evaluateState();
        }
    }

    public void resetPhysics()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    protected abstract void init();
    protected abstract void update();
    protected abstract void refresh();
}
