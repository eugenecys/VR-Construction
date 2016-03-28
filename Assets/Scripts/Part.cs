using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Part : MonoBehaviour
{
    AssetManager assetManager;

    public enum State
    {
        Unconnectable,
        Connectable,
        Free,
        Placed,
    }

    public bool triggerable;

    Segment[] segments;
    Weapon[] weapons;

    public State state;

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
        segments = GetComponentsInChildren<Segment>();
        weapons = GetComponentsInChildren<Weapon>();
        foreach (Segment cpt in segments)
        {
            cpt.parent = this;
        }
        state = State.Free;
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
        }
    }

    public void deploy()
    {
        foreach (Segment cpt in segments)
        {
            cpt.deploy();
        }
    }
    
    public void activate()
    {
        foreach (Segment cpt in segments)
        {
            cpt.activate();
        }
    }

    public void reset()
    {
        foreach (Segment cpt in segments)
        {
            cpt.reset();
        }
    }

    public void trigger()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.fire();
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
        }
    }

    public void setSegmentMaterials(Material material)
    {
        foreach (Segment cpt in segments)
        {
            cpt.setMaterial(material);
        }
    }

    public void setSegmentDefaultMaterials()
    {
        foreach (Segment cpt in segments)
        {
            cpt.setDefaultMaterial();
        }
    }

    public void evaluateState()
    {
        if (placed)
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
    /*
    public bool hasUnconnectableTouching()
    {
        if (segments == null)
        {
            return false;
        }
        for (int i = 0; i < segments.Length; i++)
        {
            if (!segments[i].connectable && segments[i].touchingSegments.Count > 0)
            {
                return true;
            }
        }
        return false;
    }
    */
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
