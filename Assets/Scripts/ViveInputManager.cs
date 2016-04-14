using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ViveInputManager : Singleton<ViveInputManager>
{
    public SteamVR_TrackedObject leftControllerObject;
    public SteamVR_TrackedObject rightControllerObject;

    int leftControllerIndex = -1;
    int rightControllerIndex = -1;

    private bool leftTriggerOn;
    private bool leftTouchpadOn;
    private bool leftApplicationmenuOn;
    private bool rightTriggerOn;
    private bool rightTouchpadOn;
    private bool rightApplicationmenuOn;

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
        RightApplicationMenuUp,

        LeftTriggerAndTouchpad,
        RightTriggerAndTouchpad
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
        if (leftControllerIndex != -1)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input(leftControllerIndex);

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.LeftTriggerDown))
            {
                inputMap[InputType.LeftTriggerDown]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.LeftTouchpadDown))
            {
                if (leftTriggerOn && inputMap.ContainsKey(InputType.LeftTriggerAndTouchpad))
                {
                    inputMap[InputType.LeftTriggerAndTouchpad]();
                } 
                else
                {
                    inputMap[InputType.LeftTouchpadDown]();
                }
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
        else
        {
            leftControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
            leftControllerObject.index = (SteamVR_TrackedObject.EIndex)leftControllerIndex;
            leftControllerObject.isValid = true;
        }
    }

    void updateRight()
    {
        if (rightControllerIndex != -1)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input(rightControllerIndex);

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.RightTriggerDown))
            {
                inputMap[InputType.RightTriggerDown]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.RightTouchpadDown))
            {
                if (rightTriggerOn && inputMap.ContainsKey(InputType.RightTriggerAndTouchpad))
                {
                    inputMap[InputType.RightTriggerAndTouchpad]();
                }
                else
                {
                    inputMap[InputType.RightTouchpadDown]();
                }
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
        else
        {
            rightControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
            rightControllerObject.index = (SteamVR_TrackedObject.EIndex)rightControllerIndex;
            rightControllerObject.isValid = true;
        }
    }
}
