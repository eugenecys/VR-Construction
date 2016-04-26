﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RobotPowerUI : MonoBehaviour {

	public Image battery;
	public Image fill; 
	public Image empty;
	public Text robotPower;

	private Robot robot; 

	[Range(0f,1f)] public float lowPower = 0.3f;

	// Use this for initialization
	void Start () {
		robot = Robot.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePowerLevel ();
	}

	private void UpdatePowerLevel() {
		float powerRatio = (robot.currentPowerLevel*1f) / (1f*robot.maxPowerLevel);
		if (powerRatio <= lowPower) {
			battery.enabled = false;
			empty.enabled = true;
		} else {
			battery.enabled = true;
			empty.enabled = false;
		} 
		fill.fillAmount = powerRatio;
		robotPower.text = "Robot\nPower : " + ((int)(powerRatio * 100f)).ToString () + "%";
	}
}
