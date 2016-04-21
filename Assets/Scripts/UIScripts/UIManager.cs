using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager :  Singleton<UIManager> {

	 

	public GameObject StartUI;
	public GameObject SelectBaseUI;
	public GameObject BuildingUI;
	public GameObject DeployedUI;
	public GameObject PickUpUI;
	public GameObject ScaleUI;
	public GameObject MovementUI;
	public GameObject WeaponsUI;
	public Text ScoreUI; 

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
		UpdateScore ();
	}
		
	public void StartGame() {
		StartUI.SetActive (false);
		SelectBaseUI.SetActive (true);
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
		ShowScore (true);
	}

	public void UndeployRobot() {
		BuildingUI.SetActive (true);
		DeployedUI.SetActive (false);
		ShowScore (false);
	}

	public void ShowPickUpControls(bool val) {
		PickUpUI.SetActive (val);
	}

	public void ShowScaleControls(bool val) {
		//ScaleUI.SetActive (val);
	}
		
	public void ShowWeaponsControls(bool val) {
		WeaponsUI.SetActive (val);
	}

	public void ShowMovementControls(bool val) {
		MovementUI.SetActive (val);
	}

	private void UpdateScore() {
		ScoreUI.text = "Score : " + ScoreManager.Instance._score;
	}

	public void ShowScore(bool val) {
		ScoreUI.gameObject.SetActive (val);
	}
}
