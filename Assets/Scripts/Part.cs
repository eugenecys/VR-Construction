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
        Inactive,
        Active,
        Deployed
    }

    Component[] components;
    public State state;

    public bool placed
    {
        get
        {
            return (state.Equals(State.Active) || state.Equals(State.Deployed) || state.Equals(State.Inactive));
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
        components = GetComponentsInChildren<Component>();
        foreach (Component cpt in components)
        {
            cpt.parent = this;
        }
        state = State.Free;
    }

    public void connect()
    {
        if (connectable)
        {
            foreach (Component component in components)
            {
                component.connect();
            }
            deploy();
        }
    }

    public void setState(State _state)
    {
        state = _state;
        switch (_state)
        {
            case State.Connectable:
                setComponentMaterials(assetManager.connectableMaterial);
                break;
            case State.Unconnectable:
                setComponentMaterials(assetManager.unconnectableMaterial);
                break;
            case State.Free:
                setComponentMaterials(assetManager.freeMaterial);
                break;
            case State.Active:
                setComponentDefaultMaterials();
                break;
            case State.Inactive:
                setComponentDefaultMaterials();
                break;
            case State.Deployed:
                setComponentDefaultMaterials();
                break;
        }
    }

    public void setComponentMaterials(Material material)
    {
        foreach (Component cpt in components)
        {
            cpt.setMaterial(material);
        }
    }

    public void setComponentDefaultMaterials()
    {
        foreach (Component cpt in components)
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
            if (hasComponentOverlap())
            {
                setState(State.Unconnectable);
                return;
            }
            if (hasTouchingComponents())
            {
                setState(State.Connectable);
                return;
            }
            setState(State.Free);
        }
    }

    public void deploy()
    {
        foreach (Component cpt in components)
        {
            cpt.deploy();
        }
        setState(State.Deployed);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool hasTouchingComponents()
    {
        if (components == null)
        {
            return false;
        }
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].touchingComponents.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool hasComponentOverlap()
    {
        if (components == null)
        {
            return false;
        }
        for (int i = 0; i < components.Length; i++)
        {
            List<Component> tCpts = components[i].touchingComponents;
            foreach (Component a in tCpts)
            {
                foreach (Component b in tCpts)
                {
                    if (!a.Equals(b) && a.parent.Equals(b.parent))
                    {
                        return true;
                    }
                }
            }
        }

        if (components.Length > 1)
        {
            for (int i = 0; i < components.Length - 1; i++)
            {
                for (int j = 1; j < components.Length; j++)
                {
                    if (i != j)
                    {
                        if (hasTouchingComponentOverlap(components[i], components[j]))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
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

}
