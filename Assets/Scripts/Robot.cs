using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : Singleton<Robot> {

    Part[] parts;

    public enum State
    {
        Inactive,
        Active,
        Deployed
    }

    public State state;

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
}
