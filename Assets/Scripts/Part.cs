﻿using UnityEngine;
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
        Template
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

    public Segment[] segments;
    Weapon[] weapons;
    MaterialHandler[] materialHandlers;
    List<Part> connectedParts;

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
        
        foreach (Segment cpt in segments)
        {
            cpt.parent = this;
        }
        if (template)
        {
            setState(State.Template);
            activate();
        }
        else
        {
            setState(State.Free);
        }

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
            enablePhysics();
            resetPhysics();
        }
        else if (free)
        {
            setState(State.Placed);
            this.transform.parent = robot.transform;
            enablePhysics();
            resetPhysics();
        }
        robot.updateParts();
    }

    public void unplace()
    {
        disablePhysics();
        resetPhysics();
        robot.updateParts();
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
                setSegmentMaterials(assetManager.connectableMaterial);
                break;
            case State.Unconnectable:
                setSegmentMaterials(assetManager.unconnectableMaterial);
                break;
            case State.Free:
                setSegmentMaterials(assetManager.freeMaterial);
                break;
            case State.Placed:
                setSegmentDefaultMaterials();
                break;
            case State.Template:
                if (highlighted)
                {
                    setSegmentMaterials(assetManager.highlightMaterial);
                }
                else
                {
                    setSegmentDefaultMaterials();
                }
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
            setSegmentMaterials(assetManager.highlightMaterial);
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

    public void evaluateState()
    {
        if (template)
        {
            setState(State.Template);
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
