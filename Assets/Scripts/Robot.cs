using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : Singleton<Robot>, Controllable {

    Part[] parts;
	Weapon[] weapons;
    public enum State
    {
        Inactive,
        Active,
        Deployed
    }

    public State state;

	public int maxPowerLevel = 1000;
	public int currentPowerLevel = 1000;

    public void destroy()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void trigger()
    {
        foreach (Part part in parts)
        {
            if (part.controllable)
            {
                part.trigger();
				if (!UIManager.firedForFirstTime) {
					UIManager.Instance.ShowWeaponsControls (false);
					UIManager.firedForFirstTime = true;
				}
            }
        }
    }

    public void triggerStop()
    {
        foreach (Part part in parts)
        {
            if (part.controllable)
            {
                part.triggerStop();
            }
        }
    }
    public void joystick(Vector2 vec)
    {
        foreach (Part part in parts)
        {
            if (part.controllable)
            {
                part.joystick(vec);

				if (!UIManager.movedForFirstTime) {
					UIManager.Instance.ShowMovementControls (false);
					UIManager.movedForFirstTime = true;
				}
            }
        }
    }

    public void joystickStop()
    {
        foreach (Part part in parts)
        {
            if (part.controllable)
            {
                part.joystickStop();
            }
        }
    }

    public void deploy()
    {
        state = State.Deployed;
        parts = GetComponentsInChildren<Part>();
        foreach (Part part in parts)
        {
			part.deploy(true);
        }
    }

    public void activate()
    {
        state = State.Active;
        parts = GetComponentsInChildren<Part>();
        foreach (Part part in parts)
        {
			part.activate ();
        }
    }

    public void reset()
    {
        state = State.Inactive;
        parts = GetComponentsInChildren<Part>();
        foreach (Part part in parts)
        {
            part.reset();
        }
    }

	public void online()
	{
		state = State.Active;
		parts = GetComponentsInChildren<Part>();
		foreach (Part part in parts)
		{
			part.online ();
		}
	}

    void Awake ()
    {
        parts = new Part[0];
    }
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void updateParts()
    {
        parts = GetComponentsInChildren<Part>();
		weapons = GetComponentsInChildren<Weapon> ();
		currentPowerLevel = 1000;
		foreach (Weapon weapon in weapons) {
			currentPowerLevel -= weapon.powerUsed;
		}
    }
}
