using UnityEngine;
using System.Collections;

public class UIManager :  Singleton<UIManager> {

	 

	public GameObject StartUI;
	public GameObject SelectBaseUI;
	public GameObject BuildingUI;
	public GameObject DeployedUI;
	public GameObject PickUpUI;
	public GameObject ScaleUI;
	public GameObject MovementUI;
	public GameObject WeaponsUI;

	public static bool pickedUpForFirstTime = false;
	public static bool scaledForFirstTime = false;
	public static bool movedForFirstTime = false;
	public static bool firedForFirstTime = false;
	// Use this for initialization
	void Start () {
		if (GameManager.Instance.state == GameManager.GameState.Start) {
			StartUI.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		KeyControls ();
	}
		
	public void StartGame() {
		StartUI.SetActive (false);
		SelectBaseUI.SetActive (true);
	}

	private void KeyControls() {

		if (Input.GetKeyDown (KeyCode.O)) {
			SelectBaseOne ();
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			SelectBaseTwo ();
		}
	}

	public void SelectBaseOne() {
		GameManager.Instance.SelectBase (1);
		SelectBaseUI.SetActive (false);
		BuildingUI.SetActive (true);
		ShowPickUpControls (true);
	}

	public void SelectBaseTwo() {
		GameManager.Instance.SelectBase (2);
		SelectBaseUI.SetActive (false);
		BuildingUI.SetActive (true);
		ShowPickUpControls (true);
	}

	public void DeployRobot() {
		BuildingUI.SetActive (false);
		DeployedUI.SetActive (true);
		ShowPickUpControls(false);
		ShowScaleControls(false);
	}

	public void UndeployRobot() {
		BuildingUI.SetActive (true);
		DeployedUI.SetActive (false);
		ShowWeaponsControls (false);
		ShowMovementControls (false);
	}

	public void ShowPickUpControls(bool val) {
		PickUpUI.SetActive (val);
	}

	public void ShowScaleControls(bool val) {
		ScaleUI.SetActive (val);
	}
		
	public void ShowWeaponsControls(bool val) {
		WeaponsUI.SetActive (val);
	}

	public void ShowMovementControls(bool val) {
		MovementUI.SetActive (val);
	}
}
