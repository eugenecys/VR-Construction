using UnityEngine;
using System.Collections;

public class PlayMode : MonoBehaviour{
    public enum InputType {
        triggerDown = 1,
        touchpadDown = 2,
        triggerUp = 3,
        touchpadUp = 4
    }
    
    private InputType curInput;

    private Robot robot;

    void Awake() {
        robot = Robot.Instance;
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetController(OmniTool.Side side, InputType type) {
        var deviceIndex = side == OmniTool.Side.Left ? SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost) : SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        curInput = type;

        switch (curInput) {
            case InputType.triggerDown:
                TriggerDown(deviceIndex);
                break;
            case InputType.touchpadDown:
                TouchPadDown(deviceIndex);
                break;
            case InputType.triggerUp:
                TriggerUp(deviceIndex);
                break;
            case InputType.touchpadUp:
                TouchPadUp(deviceIndex);
                break;
        }
    }

    void TriggerDown(int index) {
        robot.trigger();
    }

    void TouchPadDown(int index) {
        robot.move();
    }

    void TouchPadUp(int index) {
        robot.stop();
    }

    void TriggerUp(int index) {
    }
}
