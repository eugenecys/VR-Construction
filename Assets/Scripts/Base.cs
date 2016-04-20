﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class Base : Segment, Controllable
{

    private AudioSource audioSource;
    private SoundManager soundManager;
    public HingeJoint[] leftWheels;
    public HingeJoint[] rightWheels;
	private List<Rigidbody> wheelRBs;
    
    // Use this for initialization
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
        soundManager = SoundManager.Instance;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundManager.wheelSound;
		wheelRBs = new List<Rigidbody> ();
		foreach (HingeJoint wheel in leftWheels) {
			wheelRBs.Add(wheel.gameObject.GetComponent<Rigidbody>());
		}
		foreach (HingeJoint wheel in rightWheels) {
			wheelRBs.Add(wheel.gameObject.GetComponent<Rigidbody>());
		}
    }

    void Start()
    {
        active = false;
        parent.evaluateState();
        init();
    }

    protected override void init()
    {
        off();
    }

    public void initProperties()
    {
    }

    protected override void update()
    {

    }

    protected override void refresh()
    {
        if (parent.template)
        {
            if (active)
            {
                on();
            }
            else
            {
                off();
            }
        }
        else {
            switch (robot.state)
            {
                case Robot.State.Active:
                    on();
                    break;
                case Robot.State.Inactive:
                    off();
                    break;
                case Robot.State.Deployed:
                    on();
                    break;
            }
        }
    }

    public void setAngularVelocity(HingeJoint wheel, float vel)
    {
        JointMotor motor = wheel.motor;
        motor.targetVelocity = vel;
        wheel.motor = motor;
    }

    public void setAngularForce(HingeJoint wheel, float force)
    {
        JointMotor motor = wheel.motor;
        motor.force = force;
        wheel.motor = motor;
    }

    public void on()
    {
		foreach (Rigidbody rb in wheelRBs) {
			rb.useGravity = true;
			rb.isKinematic = false;
		}
        foreach (HingeJoint joint in leftWheels)
        {
            joint.useMotor = true;
        }
        foreach (HingeJoint joint in rightWheels)
        {
            joint.useMotor = true;
        }
        initProperties();
    }

    public void off()
	{
		foreach (Rigidbody rb in wheelRBs) {
			rb.useGravity = false;
			rb.isKinematic = true;
		}
        foreach (HingeJoint joint in leftWheels)
        {
            joint.useMotor = false;
        }
        foreach (HingeJoint joint in rightWheels)
        {
            joint.useMotor = false;
        }
        initProperties();
    }

    public void trigger()
    {

    }

    public void joystick(Vector2 coordinates)
	{
        int direction = (int)(coordinates.y / Mathf.Abs(coordinates.y));
        float magnitude = coordinates.magnitude;
        float leftSpeed = 0;
        float rightSpeed = 0;
		float leftForce = 0;
		float rightForce = 0;
        if (coordinates.x > 0)
        {
            leftSpeed = magnitude * direction;
			rightSpeed = coordinates.y;

        }
        else
        {
            rightSpeed = magnitude * direction;
            leftSpeed = coordinates.y;
		}
		leftForce = 1;
		rightForce = 1;
		//Debug.Log ("left: " + leftSpeed + ", " + rightSpeed);
        setLeftSpeed(leftSpeed, leftForce);
		setRightSpeed(rightSpeed, rightForce);
        //audioSource.Play();
    }

	void setLeftSpeed(float speed, float force)
    {
        foreach (HingeJoint wheel in leftWheels)
        {
			setAngularForce (wheel, force * Constants.Wheel.FORCE);
            setAngularVelocity(wheel, speed * Constants.Wheel.ANGULAR_VELOCITY);
        }
    }

	void setRightSpeed(float speed, float force)
    {
        foreach (HingeJoint wheel in rightWheels)
		{
			setAngularForce (wheel, force * Constants.Wheel.FORCE);
            setAngularVelocity(wheel, - speed * Constants.Wheel.ANGULAR_VELOCITY);
        }
    }

    public void triggerStop()
    {

    }

    public void joystickStop()
    {
        foreach (HingeJoint wheel in leftWheels)
        {
            setAngularVelocity(wheel, 0);
        }
        foreach (HingeJoint wheel in rightWheels)
        {
            setAngularVelocity(wheel, 0);
        }
        audioSource.Stop();
    }
}