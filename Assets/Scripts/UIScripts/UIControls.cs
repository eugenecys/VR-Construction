 using UnityEngine;
using System.Collections;

public class UIControls : MonoBehaviour {


	private LaserPointer laser; 
	private bool triggered = false;

	// Use this for initialization
	void Start () {
		laser = this.gameObject.GetComponent<LaserPointer> ();
	}

	// Update is called once per frame
	void Update () { 
		
	}

	public void triggerDown()
	{
		if (triggered) {
			return;
		}
		triggered = true;
		if (laser.previousContact != null) {
			UIKey keyBtn = laser.previousContact.GetComponent<UIKey> ();
			VRButton vrBtn = laser.previousContact.GetComponent<VRButton> ();
			BaseButton baseBtn = laser.previousContact.GetComponent<BaseButton> ();
			if (keyBtn) {
				keyBtn.keySelected ();
			} else if (vrBtn) {
				vrBtn.btnFunction.Invoke ();
			} else if (baseBtn) {
				baseBtn.btnFunction.Invoke ();
			} 
		} else {
			
		}
	}

	public void triggerUp() {
		if (!triggered) {
			return;
		}
		triggered = false;

	}
}
