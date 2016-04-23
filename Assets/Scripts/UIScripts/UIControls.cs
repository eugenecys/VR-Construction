﻿ using UnityEngine;
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
			UIKey keyBtn = laser.previousContact.GetComponent<UIKey> ();
			VRButton vrBtn = laser.previousContact.GetComponent<VRButton> ();
			BaseButton baseBtn = laser.previousContact.GetComponent<BaseButton> ();
			if (keyBtn) {
				keyBtn.keySelected ();
				Debug.Log ("pressed keyboard");
			}
			else if (vrBtn) {
				vrBtn.btnFunction.Invoke ();
			} else if (baseBtn){
				baseBtn.btnFunction.Invoke ();
			} 

		}
	}

	public void triggerUp() {

	}
}
