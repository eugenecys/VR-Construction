using UnityEngine;
using System.Collections;

public class UIManager :  Singleton<UIManager> {

	 

	public GameObject StartUI;
	public GameObject SelectBaseUI;
	public GameObject BuildingUI;
	public GameObject DeploymentUI;
	public GameObject ControlsUI;

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

	}

	public void SelectBaseTwo() {
		GameManager.Instance.SelectBase (2);
		SelectBaseUI.SetActive (false);
	}
		
}
