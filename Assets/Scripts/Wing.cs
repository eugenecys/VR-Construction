using UnityEngine;
using System.Collections;

public class Wing : Segment, Controllable {
    public float duration;
    public Rigidbody pillar;
    private bool isPressed;
    private Vector2 curCoor;

    protected override void init()
    {
        initProperties();
    }

    public void initProperties()
    {
        setAngularForce(0);
        setAngularVelocity(0);
    }

    void Update() {
        update();
    }

    protected override void update()
    {
        if (isPressed)
        {
            pillar.AddForce(pillar.transform.right * Constants.Wing.FORCE, ForceMode.VelocityChange);
            
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
        
    }

    public void setAngularForce(float force)
    {
        
    }

    public void on()
    {
        initProperties();
    }

    public void off()
    {
        initProperties();
    }

    public void trigger()
    {

    }

    public void joystick(Vector2 coordinates)
    {
        isPressed = true;
        curCoor = coordinates;
    }

    public void triggerStop()
    {

    }

    public void joystickStop()
    {
        //setAngularForce(0);
        isPressed = false;
    }

}
