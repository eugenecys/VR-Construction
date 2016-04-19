 using UnityEngine;
using System.Collections;

public class UIControls : MonoBehaviour {


	private LaserPointer laser; 
	// Use this for initialization
	void Start () {
		laser = this.gameObject.GetComponent<LaserPointer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void triggerDown()
	{
		if (laser.previousContact != null) {
			VRButton vrBtn = laser.previousContact.GetComponent<VRButton> ();
			if (vrBtn) {
				vrBtn.btnFunction.Invoke ();
			} else {
				BaseButton baseBtn = laser.previousContact.GetComponent<BaseButton> ();
				if (baseBtn) {
					baseBtn.btnFunction.Invoke ();
				}
			}
		}
	}

	public void triggerUp() {

	}
}
