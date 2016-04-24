using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Rigidbody))]

public abstract class Segment : MonoBehaviour {

    protected AssetManager assetManager;
    protected Robot robot;
    protected ScoreManager scoreManager;

    public Part parent;
    public List<Segment> connectedSegments;
    public List<Segment> touchingSegments;
    protected Collider col;
    protected Rigidbody rb;
    protected Collider detector;

    protected bool active;
    
	public bool isConnectedToRobot = false;
    public void connect()
    {
        foreach (Segment touchingSegment in touchingSegments)
        {
            if (!touchingSegment.isConnected(this) && !touchingSegment.parent.unconnectable)
            {
                FixedJoint fJoint = gameObject.AddComponent<FixedJoint>();
                fJoint.connectedBody = touchingSegment.rb;
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
        
        refresh();
    }

    public void disconnect()
    {
        foreach(Segment connectedSegment in connectedSegments)
        {
            connectedSegment.disconnectFrom(this);
        }
        FixedJoint[] joints = gameObject.GetComponents<FixedJoint>();
        foreach(FixedJoint joint in joints)
        {
            Destroy(joint);
        }
        connectedSegments = new List<Segment>();
    }

    public void disconnectFrom(Segment other)
    {
        FixedJoint[] joints = gameObject.GetComponents<FixedJoint>();
        FixedJoint marked = null;
        foreach (FixedJoint joint in joints)
        {
            if (joint.connectedBody.gameObject.Equals(other.gameObject))
            {
                marked = joint;
            }
        }
        if (marked != null) { 
            Destroy(marked);
        }
        connectedSegments.Remove(other);
    }

	// Use this for initialization

	/*
	void Awake () {
		
        assetManager = AssetManager.Instance;
        connectedSegments = new List<Segment>();
        touchingSegments = new List<Segment>();
        robot = Robot.Instance;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        detector = GetComponentInChildren<Collider>();
        rb.isKinematic = true;
	}

    void Start()
    {
        active = false;
        parent.evaluateState();
        init();
    }
*/

    public void enablePhysics()
    {
        rb.isKinematic = false;
    }

    public void disablePhysics()
    {
        rb.isKinematic = true;
    }

    public void deploy()
    {
        //trigger.isTrigger = false;
        rb.useGravity = true;
        enablePhysics();
        resetPhysics(); 
        active = true;
        refresh();
    }

    public void activate()
    {
        //trigger.isTrigger = false;
        rb.useGravity = false;
        disablePhysics();
        resetPhysics(); 
        active = true;
        refresh();
    }

    public void reset()
    {
        //trigger.isTrigger = true;
        rb.useGravity = false;
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
            AddScore();
        }
    }


    void OnTriggerStay(Collider other)
    {
		
		if (parent.template && other.GetComponentInParent<Part> ()) {
			return;
		} else if (parent.template) {
			parent.highlight ();
		} else if (!parent.placed && other.gameObject.tag == Constants.LAYER_COMPONENT) {
			Segment segment = other.GetComponent<Segment> ();
			if (segment == null) {
				segment = other.GetComponentInParent<Segment> ();
			}

			updateTouchingSegments (segment);
			parent.evaluateState ();
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
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected void AddScore()
    {
        if (scoreManager)
        {
            scoreManager.AddScore();
        }
    }

    protected abstract void init();
    protected abstract void update();
    protected abstract void refresh();

    //Xiqiao Add
    public virtual void move() { }
    public virtual void stop() { }
}
