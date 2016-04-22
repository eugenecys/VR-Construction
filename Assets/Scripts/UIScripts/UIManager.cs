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
	public Text TimeUI;
	public GameObject HighScoreUI;

	public static bool pickedUpForFirstTime = false;
	public static bool scaledForFirstTime = false;
	public static bool movedForFirstTime = false;
	public static bool firedForFirstTime = false;

	// Use this for initialization
	void Awake () {
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
		ShowTime (true);
	}

	public void UndeployRobot() {
		BuildingUI.SetActive (true);
		DeployedUI.SetActive (false);
		ShowScore (false);
		ShowTime (false);
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

	private void UpdateScore() {
		ScoreUI.text = "Score : " + ScoreManager.Instance._score;
	}

	public void ShowScore(bool val) {
		ScoreUI.gameObject.SetActive (val);
	}

	public void ShowTime(bool val) {
		TimeUI.gameObject.SetActive (val);
	}
		

	public void EndGame(int finalScore) {
		ShowTime (false);
		HighScoreUI.SetActive (true);
		Text rank; 
		for (int i = 0; i < 10; i++) {
			rank = HighScoreUI.transform.GetChild (i).GetComponent<Text> ();
			rank.text += PlayerPrefs.GetInt((i+1).ToString()).ToString();
		}
		HighScoreUI.transform.GetChild (10).GetComponent<Text> ().text += finalScore.ToString();
	
	}
}
