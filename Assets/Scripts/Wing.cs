using UnityEngine;
using System.Collections;

public class Wing : Segment, Controllable {
    public float duration;
    public Rigidbody pillar;
    protected override void init()
    {
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
        //setAngularForce(Constants.Wheel.FORCE * coordinates.magnitude);

        //setAngularVelocity(Constants.Propeller.ANGULAR_VELOCITY * coordinates.magnitude);
        StartCoroutine(WingForce(coordinates));
        
    }

    IEnumerator WingForce(Vector2 coordinates)
    {
        pillar.AddForce(pillar.gameObject.transform.up * Constants.Propeller.FORCE * coordinates.magnitude, ForceMode.VelocityChange);
        yield return new WaitForSeconds(duration);
        pillar.velocity = Vector3.zero;
        pillar.angularVelocity = Vector3.zero;
    }

    public void triggerStop()
    {

    }

    public void joystickStop()
    {
        //setAngularForce(0);
        StopCoroutine(WingForce(new Vector2(0,0)));
    }

}
