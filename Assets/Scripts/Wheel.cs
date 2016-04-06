using UnityEngine;
using System.Collections;

public class Wheel : Segment {

    public HingeJoint wheel;

    public bool reverse;

    protected override void init()
    {
        wheel.useMotor = false;
        initProperties();
    }

    public void initProperties()
    {
        setAngularForce(Constants.Wheel.FORCE);
        setAngularVelocity(Constants.Wheel.ANGULAR_VELOCITY);
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
}
