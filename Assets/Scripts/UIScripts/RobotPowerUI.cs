using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RobotPowerUI : MonoBehaviour {

	public Image battery;
	public Image fill; 
	public Image empty;
	public Text robotPower;
	public Text cannonPower;
	public Text laserPower;
	public Text drillPower;
	public Text gunPower;

	private Robot robot; 

	[Range(0f,1f)] public float lowPower = 0.3f;

	// Use this for initialization
	void Awake () {
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
		robotPower.text = "Power\nAvailable : " + ((int)(powerRatio * 100f)).ToString () + "%";
	}

	public void SetWeaponPowerPercentages() {
		robot = Robot.Instance;
		cannonPower.text = ((int) ((300f/robot.maxPowerLevel) *100f)).ToString() + "%";
		laserPower.text = ((int) ((500f/robot.maxPowerLevel) *100f)).ToString() + "%";
		gunPower.text = ((int) ((100f/robot.maxPowerLevel) *100f)).ToString() + "%";
		drillPower.text = ((int) ((50f/robot.maxPowerLevel) *100f)).ToString() + "%";
	}
}
