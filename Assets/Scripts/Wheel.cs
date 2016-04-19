using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AudioSource))]

public class Wheel : Segment, Controllable {
    
    private AudioSource audioSource;
    private SoundManager soundManager;
    public HingeJoint wheel;

    public bool reverse;

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
        wheel.useMotor = false;
        initProperties();
    }

    public void initProperties()
    {
        setAngularForce(0);
        setAngularVelocity(0);
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
            } else
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
    
    public void setAngularVelocity(float vel)
    {
        JointMotor motor = wheel.motor;
        motor.targetVelocity = reverse ? -vel : vel;
        wheel.motor = motor;
    }

    public void setAngularForce(float force)
    {
        JointMotor motor = wheel.motor;
        motor.force = force;
        wheel.motor = motor;
    }

    public void on()
    {
        wheel.useMotor = true;
        initProperties();
    }

    public void off()
    {
        wheel.useMotor = false;
        initProperties();
    }

    public void trigger()
    {
        
    }

    public void joystick(Vector2 coordinates)
    {
        setAngularForce(Constants.Wheel.FORCE);
        setAngularVelocity(Constants.Wheel.ANGULAR_VELOCITY);
        audioSource.Play();
    }

    public void triggerStop()
    {
        
    }

    public void joystickStop()
    {
        audioSource.Stop();
        setAngularForce(0);
        setAngularVelocity(0);
    }

}
