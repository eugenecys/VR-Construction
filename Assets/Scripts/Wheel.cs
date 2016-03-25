using UnityEngine;
using System.Collections;

public class Wheel : Component {

    public HingeJoint wheel;

    public void setAngularVelocity(float vel)
    {
        JointMotor motor = wheel.motor;
        motor.targetVelocity = vel;
    }

    public void setAngularForce(float force)
    {
        JointMotor motor = wheel.motor;
        motor.force = force;
    }

    public void on()
    {
        wheel.useMotor = false;
    }

    public void off()
    {
        wheel.useMotor = true;
    }

}
