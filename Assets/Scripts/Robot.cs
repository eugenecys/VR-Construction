using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : Singleton<Robot> {

    private List<Part> parts;

    public void deploy()
    {
        foreach (Part part in parts)
        {
            part.deploy();
        }
    }

    public void activate()
    {
        foreach (Part part in parts)
        {
            part.activate();
        }
    }

    public void reset()
    {
        foreach (Part part in parts)
        {
            part.reset();
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
