using UnityEngine;
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
        //setAngularForce(0);
        //setAngularVelocity(0);
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
        
        audioSource.Play();
    }

    public void triggerStop()
    {

    }

    public void joystickStop()
    {
        audioSource.Stop();
    }
}
