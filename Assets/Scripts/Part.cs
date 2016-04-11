using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Part : MonoBehaviour, Interactable
{
    AssetManager assetManager;
    Robot robot;

    public enum State
    {
        Unconnectable,
        Connectable,
        Free,
        Placed,
        MarkedForDelete,
        Highlight
    }

    public enum Name
    {
        Wheel,
        Gun,
        Chain,
        Cube,
        Rod,
        WheelR,
        Propeller,
        Saw,
        //Xiqiao Add
        Wing
    }

    public bool triggerable;
    public bool template;
    private bool highlighted;
    public bool markedForDelete;
    
    public Segment[] segments;
    Weapon[] weapons;
    MaterialHandler[] materialHandlers;
    List<Part> connectedParts;
    public Scaler scaler;

    public State state;
    public Name name;
    
    public bool placed
    {
        get
        {
            return state.Equals(State.Placed);
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

    public bool free
    {
        get
        {
            return state.Equals(State.Free);
        }
    }

    void Awake()
    {
        assetManager = AssetManager.Instance;
        robot = Robot.Instance;
        segments = GetComponentsInChildren<Segment>();
        materialHandlers = GetComponentsInChildren<MaterialHandler>();
        weapons = GetComponentsInChildren<Weapon>();
        connectedParts = new List<Part>();

        markedForDelete = false;
        highlighted = false;

        foreach (Segment cpt in segments)
        {
            cpt.parent = this;
        }

        scaler.gameObject.SetActive(false);
    }
    
    public void addConnectedPart(Part part)
    {
        foreach(Part storedPart in connectedParts)
        {
            if (part.Equals(storedPart))
            {
                return;
            }
        }
        connectedParts.Add(part);
    }

    public List<Part> getConnectedParts()
    {
        List<Part> parts = new List<Part>();
        parts.Add(this);
        foreach (Part part in connectedParts)
        {
            part.retrieveConnectedParts(parts);
        }
        return parts;
    }

    protected void retrieveConnectedParts(List<Part> existingParts)
    {
        if (existingParts.Contains(this))
        {
            return;
        }
        else
        {
            existingParts.Add(this);
        }
        foreach (Part part in connectedParts)
        {
            part.retrieveConnectedParts(existingParts);
        }
    }

    public void resetPhysics()
    {
        foreach (Segment segment in segments)
        {
            segment.resetPhysics();
        }
    }

    public void enablePhysics()
    {
        foreach(Segment segment in segments)
        {
            segment.enablePhysics();
        }
    }

    public void disablePhysics()
    {
        foreach (Segment segment in segments)
        {
            segment.disablePhysics();
        }
    }

    public void place()
    {
        if (connectable)
        {
            foreach (Segment segment in segments)
            {
                segment.connect();
            } 
            setState(State.Placed);
            this.transform.parent = robot.transform;
            resetPhysics();
        }
        else if (free)
        {
            setState(State.Placed);
            this.transform.parent = robot.transform;
            resetPhysics();
        }
        robot.updateParts();
    }

    public void unplace()
    {
        disablePhysics();
        resetPhysics();
        robot.updateParts();
        setState(Part.State.Connectable);
        evaluateState();
    }

    public void deploy()
    {
        if (!template)
        {
            foreach (Segment cpt in segments)
            {
                cpt.deploy();
            }
            setState(State.Placed);
        }
    }
    
    public void activate()
    {
        foreach (Segment cpt in segments)
        {
            cpt.activate();
        }
        setState(State.Placed);
    }

    public void reset()
    {
        foreach (Segment cpt in segments)
        {
            cpt.reset();
        }
        setState(State.Placed);
    }

    public void trigger()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.fire();
        }
    }

    //Xiqiao Add
    public void move() {
        foreach (Segment cpt in segments) {
            if (cpt.movable) {
                cpt.move();
            }
        }
    }

    //Xiqiao Add
    public void stop() {
        foreach (Segment cpt in segments)
        {
            if (cpt.movable)
            {
                cpt.stop();
            }
        }
    }

    public void setState(State _state)
    {
        state = _state;
        switch (_state)
        {
            case State.Connectable:
                scaler.gameObject.SetActive(true);
                setSegmentMaterials(assetManager.connectableMaterial);
                break;
            case State.Unconnectable:
                scaler.gameObject.SetActive(true);
                setSegmentMaterials(assetManager.unconnectableMaterial);
                break;
            case State.Free:
                scaler.gameObject.SetActive(true);
                setSegmentMaterials(assetManager.freeMaterial);
                break;
            case State.Placed:
                scaler.gameObject.SetActive(false);
                setSegmentDefaultMaterials();
                break;
            case State.MarkedForDelete:
                scaler.gameObject.SetActive(false);
                setSegmentMaterials(assetManager.deleteMaterial);
                break;
            case State.Highlight:
                scaler.gameObject.SetActive(false);
                setSegmentMaterials(assetManager.highlightMaterial);
                break;
        }
    }
    
    public void setSegmentMaterials(Material material)
    {
        foreach (MaterialHandler materialHandler in materialHandlers)
        {
            materialHandler.loadMaterial(material);
        }
    }

    public void setSegmentDefaultMaterials()
    {
        foreach (MaterialHandler materialHandler in materialHandlers)
        {
            materialHandler.loadDefault();
        }
    }
    
    public void highlight()
    {
        if (template)
        {
            highlighted = true;
            setState(State.Highlight);
        }
    }

    public void unhighlight()
    {
        if (template)
        {
            highlighted = false;
            setSegmentDefaultMaterials();
        }
    }

    public void markForDelete()
    {
        markedForDelete = true;
        setState(State.MarkedForDelete);
        List<Part> connected = getConnectedParts();
        foreach(Part part in connected)
        {
            if (!part.markedForDelete)
            {
                part.markForDelete();
            }
        }
    }

    public void unmarkForDelete()
    {
        markedForDelete = false;
        setState(State.Connectable);
        List<Part> connected = getConnectedParts();
        foreach (Part part in connected)
        {
            if (part.markedForDelete)
            {
                part.unmarkForDelete();
            }
        }
    }

    public void evaluateState()
    {
        if (markedForDelete)
        {
            setState(State.MarkedForDelete);
        }
        else if (template)
        {
            activate();
        } 
        else if (placed)
        {

        }
        else
        {
            if (hasSegmentOverlap())
            {
                setState(State.Unconnectable);
                return;
            }
            if (hasTouchingSegments())
            {
                setState(State.Connectable);
                return;
            }
            setState(State.Free);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public bool hasTouchingSegments()
    {
        if (segments == null)
        {
            return false;
        }
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].touchingSegments.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool hasSegmentOverlap()
    {
        if (segments == null)
        {
            return false;
        }
        for (int i = 0; i < segments.Length; i++)
        {
            List<Segment> tCpts = segments[i].touchingSegments;
            foreach (Segment a in tCpts)
            {
                foreach (Segment b in tCpts)
                {
                    if (!a.Equals(b) && a.parent.Equals(b.parent))
                    {
                        return true;
                    }
                }
            }
        }

        if (segments.Length > 1)
        {
            for (int i = 0; i < segments.Length - 1; i++)
            {
                for (int j = 1; j < segments.Length; j++)
                {
                    if (i != j)
                    {
                        if (hasTouchingSegmentOverlap(segments[i], segments[j]))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    bool hasTouchingSegmentOverlap(Segment a, Segment b)
    {
        foreach (Segment aC in a.touchingSegments)
        {
            foreach (Segment bC in b.touchingSegments)
            {
                if (aC.Equals(bC))
                {
                    return true;
                }
            }
        }
        return false;
    }

}
