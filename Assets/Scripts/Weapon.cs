﻿using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour, Controllable {

    protected EventManager eventManager;
    
    public virtual void joystick(Vector2 coordinates)
    {

    }

    public virtual void joystickStop()
    {

    }

    public virtual void trigger()
    {
        
    }

    public virtual void triggerStop()
    {

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
