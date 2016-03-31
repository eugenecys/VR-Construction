using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViveInputManager : Singleton<ViveInputManager> {

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
        RightTouchpad
    }

    void Awake()
    {
        if(inputMap == null)
        {
            inputMap = new Dictionary<InputType, InputFunction>();
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        updateLeft();
        updateRight();
	}

    public void registerFunction(InputFunction func, InputType type)
    {
        if(inputMap == null)
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
            Debug.Log("Left: " + left);
            SteamVR_Controller.Device device = SteamVR_Controller.Input(left);
            leftControllerObject.transform.position = device.transform.pos;
            leftControllerObject.transform.rotation = device.transform.rot;

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.LeftTrigger))
            {
                Debug.Log("Left");
                inputMap[InputType.LeftTrigger]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.LeftTouchpad))
            {
                inputMap[InputType.LeftTouchpad]();
            }
        }
    }

    void updateRight()
    {
        int right = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        if (right != -1)
        {
            Debug.Log("Right: " + right);
            SteamVR_Controller.Device device = SteamVR_Controller.Input(right);
            rightControllerObject.transform.position = device.transform.pos;
            rightControllerObject.transform.rotation = device.transform.rot;

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.RightTrigger))
            {
                Debug.Log("Right");
                inputMap[InputType.RightTrigger]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.RightTouchpad))
            {
                inputMap[InputType.RightTouchpad]();
            }
        }
    }
}
