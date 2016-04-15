using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Propeller : Segment, Controllable {
    public HingeJoint propeller;
    public Rigidbody pillar;

    protected override void init()
    {
        propeller.useMotor = false;
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
            }
            else
            {
                off();
            }
        }
        else
        {
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
        JointMotor motor = propeller.motor;
        motor.targetVelocity = vel;
        propeller.motor = motor;
    }

    public void setAngularForce(float force)
    {
        JointMotor motor = propeller.motor;
        motor.force = force;
        propeller.motor = motor;
    }

    public void on()
    {
        propeller.useMotor = true;
        initProperties();
    }

    public void off()
    {
        propeller.useMotor = false;
        initProperties();
    }

    public void trigger()
    {

    }

    public void joystick(Vector2 coordinates)
    {
        //setAngularForce(Constants.Wheel.FORCE * coordinates.magnitude);
        pillar.AddForce(pillar.gameObject.transform.up * Constants.Propeller.FORCE * coordinates.magnitude, ForceMode.VelocityChange);
        setAngularVelocity(Constants.Propeller.ANGULAR_VELOCITY * coordinates.magnitude);
    }

    public void triggerStop()
    {
    }

    public void joystickStop()
    {
        //setAngularForce(0);
        setAngularVelocity(0);
    }
}
