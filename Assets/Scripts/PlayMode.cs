using UnityEngine;
using System.Collections;

public class PlayMode : MonoBehaviour{

    private Robot robot;
    private Vector2 smoothAxis;
    private int smoothing = 8;

    void Awake() {
        robot = Robot.Instance;
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    
    public void triggerDown() {
        robot.trigger();
    }

    public void touchPadDown(Vector2 vec)
    {
        smoothAxis = smoothAxis * 7 / 8 + vec / 8;
        robot.joystick(smoothAxis);
    }

    public void touchPadUp() {
        robot.joystickStop();
    }

    public void triggerUp()
    {
        robot.triggerStop();
    }
}
