using UnityEngine;
using System.Collections;

public class Wing : Segment {

    protected override void init()
    {
        initProperties();
    }

    public void initProperties()
    {
        setAngularForce(0);
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

    //Xiqiao Add
    public override void move()
    {
        setAngularForce(Constants.Wing.FORCE);
    }

    //Xiqiao Add
    public override void stop()
    {
        setAngularForce(0);
    }

    public void setAngularForce(float force)
    {
        rigidbody.AddForceAtPosition(force * transform.forward, transform.position, ForceMode.Force);
    }

    public void on()
    {
        initProperties();
    }

    public void off()
    {
        initProperties();
    }
}
