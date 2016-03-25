using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : Singleton<Robot> {

    private List<Component> components;

	// Use this for initialization
	void Start () {
        components = new List<Component>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
