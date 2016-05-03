using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RobotPowerUI : MonoBehaviour {

	public Image battery;
	public Image fill; 
	public Text robotPower;
	public Text cannonPower;
	public Text laserPower;
	public Text drillPower;
	public Text gunPower;

	private Robot robot; 


	// Use this for initialization
	void Awake () {
		robot = Robot.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePowerLevel ();
	}

	private void UpdatePowerLevel() {
		float powerRatio = 1f - (robot.currentPowerLevel*1f) / (1f*robot.maxPowerLevel);
		fill.fillAmount = powerRatio;
		if (powerRatio < 0.99f) {
			robotPower.text = ((int)(powerRatio * 100f)).ToString () + "%";
		} else {
			robotPower.text = "MAX";
		}
	}

	public void SetWeaponPowerPercentages() {
		robot = Robot.Instance;
		cannonPower.text = ((int) ((300f/robot.maxPowerLevel) *100f)).ToString() + "%";
		laserPower.text = ((int) ((500f/robot.maxPowerLevel) *100f)).ToString() + "%";
		gunPower.text = ((int) ((100f/robot.maxPowerLevel) *100f)).ToString() + "%";
		drillPower.text = ((int) ((50f/robot.maxPowerLevel) *100f)).ToString() + "%";
	}
}
