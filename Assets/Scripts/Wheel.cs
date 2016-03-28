using UnityEngine;
using System.Collections;

public class Wheel : Component {

    public HingeJoint wheel;

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
        JointMotor motor = wheel.motor;
        motor.targetVelocity = vel;
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
        Debug.Log("On");
        wheel.useMotor = true;
        initProperties();
    }

    public void off()
    {
        Debug.Log("Off");
        wheel.useMotor = false;
        initProperties();
    }
}
