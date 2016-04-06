using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViveInputManager : Singleton<ViveInputManager>
{

    SteamVR_Controller leftController;
    SteamVR_Controller rightController;

    public GameObject leftControllerObject;
    public GameObject rightControllerObject;

    public delegate void InputFunction();

    private Dictionary<InputType, InputFunction> inputMap;

    public enum InputType
    {
        LeftTrigger,
        RightTrigger,
        LeftTouchpad,
        RightTouchpad,
        LeftApplicationMenu,
        RightApplicationMenu
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
    void Update()
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

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.LeftTrigger))
            {
                inputMap[InputType.LeftTrigger]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.LeftTouchpad))
            {
                inputMap[InputType.LeftTouchpad]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.LeftApplicationMenu))
            {
                inputMap[InputType.LeftApplicationMenu]();
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

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.RightTrigger))
            {
                inputMap[InputType.RightTrigger]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.RightTouchpad))
            {
                inputMap[InputType.RightTouchpad]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.RightApplicationMenu))
            {
                inputMap[InputType.RightApplicationMenu]();
            }
        }
    }
}
