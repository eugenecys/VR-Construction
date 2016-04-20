using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (AudioSource))]
public class Part : MonoBehaviour, Interactable
{
    AudioSource audioSource;
    AssetManager assetManager;
    SoundManager soundManager;
    Robot robot;

    public enum State
    {
        Unconnectable,
        Connectable,
        Free,
        Placed,
        MarkedForDelete
    }
    
    public bool controllable;
    public bool template;
    private bool highlighted;
    public bool markedForDelete;
    public bool scalable;
    
    private Segment[] segments;
    Controllable[] controllables;
    MaterialHandler[] materialHandlers;
    List<Part> connectedParts;
    private Scaler scaler;

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
        robot = Robot.Instance;
        soundManager = SoundManager.Instance;
        segments = GetComponentsInChildren<Segment>();
        materialHandlers = GetComponentsInChildren<MaterialHandler>();
        controllables = GetComponentsInChildren<Controllable>();
        connectedParts = new List<Part>();
        scaler = GetComponentInChildren<Scaler>();
        audioSource = GetComponent<AudioSource>();

        markedForDelete = false;
        highlighted = false;

        foreach (Segment cpt in segments)
        {
            cpt.parent = this;
        }

        if (scaler == null)
        {
            scalable = false;
        }
        else
        {
            scalable = true;
            scaler.gameObject.SetActive(false);
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

    public void removeConnectedPart(Part part)
    {
		connectedParts.Remove (part);
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
            deploy();
            audioSource.PlayOneShot(soundManager.attachSound);
            this.transform.parent = robot.transform;
            resetPhysics();
        }
        else if (free)
        {
            deploy();
			audioSource.PlayOneShot(soundManager.attachSound);
            this.transform.parent = robot.transform;
            resetPhysics();
        }
        robot.updateParts();
    }

    public void disconnect()
    {
        foreach(Part part in connectedParts)
        {
            part.removeConnectedPart(this);
        }
        connectedParts = new List<Part>();

        foreach(Segment segment in segments)
        {
            segment.disconnect();
        }
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
        foreach (Controllable controllable in controllables)
        {
            controllable.trigger();
        }
    }

    public void triggerStop()
    {
        foreach (Controllable controllable in controllables)
        {
            controllable.triggerStop();
        }
    }

    public void joystick(Vector2 input)
    {
        foreach (Controllable controllable in controllables)
        {
            controllable.joystick(input);
        }
    }

    public void joystickStop()
    {
        foreach (Controllable controllable in controllables)
        {
            controllable.joystickStop();
        }
    }

    public void setState(State _state)
    {
        state = _state;
        switch (_state)
        {
            case State.Connectable:
                if (scalable)
                {
                    scaler.gameObject.SetActive(true);
                }
                setSegmentMaterials(assetManager.connectableMaterial);
                break;
            case State.Unconnectable:
                if (scalable)
                {
                    scaler.gameObject.SetActive(true);
                }
                setSegmentMaterials(assetManager.unconnectableMaterial);
                break;
            case State.Free:
                if (scalable)
                {
                    scaler.gameObject.SetActive(true);
                }
                setSegmentMaterials(assetManager.freeMaterial);
                break;
            case State.Placed:
                if (scalable)
                {
                    scaler.gameObject.SetActive(false);
                }
                setSegmentDefaultMaterials();
                break;
            case State.MarkedForDelete:
                if (scalable)
                {
                    scaler.gameObject.SetActive(false);
                }
                setSegmentMaterials(assetManager.deleteMaterial);
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
        highlighted = true;
        setSegmentMaterials(assetManager.highlightMaterial);
    }

    public void unhighlight()
    {
        highlighted = false;
        setSegmentDefaultMaterials();
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
