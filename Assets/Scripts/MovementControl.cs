using UnityEngine;
using System.Collections;

public class MovementControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        var device = SteamVR_Controller.Input(deviceIndex);
        if (deviceIndex != -1 && device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            Vector2 v = device.GetAxis();
            Vector3 newDir = new Vector3(transform.position.x + v.x, transform.position.y, transform.position.z + v.y);
            transform.Translate(newDir.normalized * Time.deltaTime * 0.05f);
        }
    }
}
