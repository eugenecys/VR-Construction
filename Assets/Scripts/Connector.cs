﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Connector : Segment {

    void Awake()
    {
        assetManager = AssetManager.Instance;
        connectedSegments = new List<Segment>();
        touchingSegments = new List<Segment>();
        robot = Robot.Instance;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        detector = GetComponentInChildren<Collider>();
        rb.isKinematic = true;

        scoreManager = ScoreManager.Instance;

    }

    void Start()
    {
        active = false;
		parent.evaluateState(false);
        init();
    }

    protected override void init()
    {
        
    }

    protected override void update()
    {
        
    }

    protected override void refresh()
    {
        
    }

}
