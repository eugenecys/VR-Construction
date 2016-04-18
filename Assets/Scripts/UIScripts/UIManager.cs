using UnityEngine;
using System.Collections;

public class UIManager :  Singleton<UIManager> {

	 

	public GameObject StartUI;
	public GameObject SelectBaseUI;
	public GameObject BuildingUI;
	public GameObject DeployedUI;
	public GameObject PickUpUI;
	public GameObject ScaleUI;


	public static bool pickedUpForFirstTime = false;
	public static bool scaledForFirstTime = false;

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
		GameManager.Instance.StartGame ();
		StartUI.SetActive (false);
		SelectBaseUI.SetActive (true);
	}

	private void KeyControls() {
		if (Input.GetKeyDown (KeyCode.I)) {
			StartGame ();
		}

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
		ShowPickUpControls ();
	}

	public void SelectBaseTwo() {
		GameManager.Instance.SelectBase (2);
		SelectBaseUI.SetActive (false);
		BuildingUI.SetActive (true);
		ShowPickUpControls ();
	}

	public void DeployRobot() {
		BuildingUI.SetActive (false);
		DeployedUI.SetActive (true);
	}

	public void UndeployRobot() {
		BuildingUI.SetActive (true);
		DeployedUI.SetActive (false);
	}

	public void ShowPickUpControls() {
		PickUpUI.SetActive (true);
	}

	public void ShowScaleControls() {
		PickUpUI.SetActive (false);
		ScaleUI.SetActive (true);
	}

	public void HideScaleControls() {
		ScaleUI.SetActive (false);
	}
}
