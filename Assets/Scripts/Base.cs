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
        int direction = (int)(coordinates.y / Mathf.Abs(coordinates.y));
        float magnitude = coordinates.magnitude;
        float leftSpeed = 0;
        float rightSpeed = 0;
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
        setLeftSpeed(leftSpeed);
        setRightSpeed(rightSpeed);
        audioSource.Play();
    }

    void setLeftSpeed(float speed)
    {
        foreach (HingeJoint wheel in leftWheels)
        {
            setAngularVelocity(wheel, speed * Constants.Wheel.ANGULAR_VELOCITY);
        }
    }

    void setRightSpeed(float speed)
    {
        foreach (HingeJoint wheel in rightWheels)
        {
            setAngularVelocity(wheel, speed * Constants.Wheel.ANGULAR_VELOCITY);
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
