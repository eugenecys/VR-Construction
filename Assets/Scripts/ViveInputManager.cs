using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViveInputManager : Singleton<ViveInputManager>
{

    SteamVR_Controller leftController;
    SteamVR_Controller rightController;

    public GameObject leftControllerObject;
    public GameObject rightControllerObject;

    public delegate void InputFunction(params object[] args);

    private Dictionary<InputType, InputFunction> inputMap;

    public enum InputType
    {
        LeftTriggerDown,
        RightTriggerDown,
        LeftTouchpadDown,
        RightTouchpadDown,
        LeftApplicationMenuDown,
        RightApplicationMenuDown,
        LeftTriggerUp,
        RightTriggerUp,
        LeftTouchpadUp,
        RightTouchpadUp,
        LeftApplicationMenuUp,
        RightApplicationMenuUp
    }

    void Awake()
    {
        if (inputMap == null)
        {
            inputMap = new Dictionary<InputType, InputFunction>();
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateLeft();
        updateRight();
    }

    public void registerFunction(InputFunction func, InputType type)
    {
        if (inputMap == null)
        {
            inputMap = new Dictionary<InputType, InputFunction>();
        }
        inputMap.Add(type, func);
    }

    void updateLeft()
    {
        int left = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        if (left != -1)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input(left);
            leftControllerObject.transform.position = device.transform.pos;
            leftControllerObject.transform.rotation = device.transform.rot;

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.LeftTriggerDown))
            {
                inputMap[InputType.LeftTriggerDown]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.LeftTouchpadDown))
            {
                inputMap[InputType.LeftTouchpadDown]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.LeftApplicationMenuDown))
            {
                inputMap[InputType.LeftApplicationMenuDown]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.LeftTriggerUp))
            {
                inputMap[InputType.LeftTriggerUp]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.LeftTouchpadUp))
            {
                inputMap[InputType.LeftTouchpadUp]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.LeftApplicationMenuUp))
            {
                inputMap[InputType.LeftApplicationMenuUp]();
            }
        }
    }

    void updateRight()
    {
        int right = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        if (right != -1)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input(right);
            rightControllerObject.transform.position = device.transform.pos;
            rightControllerObject.transform.rotation = device.transform.rot;

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.RightTriggerDown))
            {
                inputMap[InputType.RightTriggerDown]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.RightTouchpadDown))
            {
                inputMap[InputType.RightTouchpadDown]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.RightApplicationMenuDown))
            {
                inputMap[InputType.RightApplicationMenuDown]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.RightTriggerUp))
            {
                inputMap[InputType.RightTriggerUp]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.RightTouchpadUp))
            {
                inputMap[InputType.RightTouchpadUp]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.RightApplicationMenuUp))
            {
                inputMap[InputType.RightApplicationMenuUp]();
            }
        }
    }
}
