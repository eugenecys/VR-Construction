using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Propeller : Segment, Controllable {
    public HingeJoint propeller;
    public Rigidbody pillar;
    private bool isPressed;
    private Vector2 curCoor;

    protected override void init()
    {
		isPressed = false;
        propeller.useMotor = false;
		curCoor = new Vector2 (0, 0);
        initProperties();
    }

    public void initProperties()
    {
        setAngularForce(0);
        setAngularVelocity(0);
    }

	void Update(){
		update ();
	}

    protected override void update()
    {
        if (isPressed)
        {
            pillar.AddForce(pillar.gameObject.transform.up * Constants.Propeller.FORCE, ForceMode.VelocityChange);
			//pillar.velocity = pillar.transform.up * Constants.Propeller.FORCE;
			Debug.Log("pressed");
            setAngularVelocity(Constants.Propeller.ANGULAR_VELOCITY * curCoor.magnitude);
        }
        else {
            setAngularVelocity(0);
			Debug.Log ("unpressed");
			//pillar.velocity = Vector3.zero;
			//pillar.angularVelocity = Vector3.zero;
        }
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
        //setAngularForce(Constants.Wing.FORCE * coordinates.magnitude);
        isPressed = true;
        curCoor = coordinates;
    }

    public void triggerStop()
    {

    }

    public void joystickStop()
    {
        isPressed = false;
    }
}
