﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : Singleton<Robot>, Controllable {

    Part[] parts;

    public enum State
    {
        Inactive,
        Active,
        Deployed
    }

    public State state;

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
            part.deploy();
        }
    }

    public void activate()
    {
        state = State.Active;
        parts = GetComponentsInChildren<Part>();
        foreach (Part part in parts)
        {
            part.activate();
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
    }
}
