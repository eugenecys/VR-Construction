using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Propeller : Segment {

    public HingeJoint propeller;

    protected override void init()
    {
        propeller.useMotor = false;
        initProperties();
    }

    public void initProperties()
    {
        setAngularForce(Constants.Propeller.FORCE);
        setAngularVelocity(Constants.Propeller.ANGULAR_VELOCITY);
    }

    protected override void update()
    {

    }

    protected override void refresh()
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
}
