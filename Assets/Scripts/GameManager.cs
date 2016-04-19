using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {

    public enum GameState
    {
        Build,
        Play,
		Start,
		SelectBase
    }

	private AudioSource audioSource;
	private SoundManager soundManager; 
	private Robot robot;

    public GameState state;
    public GameObject city;


	public GameObject RobotBases;

	public GameObject selectedBase;

	public GameObject[] RoomComponents; 


    public void play()
    {
        state = GameState.Play;
		city.SetActive (true); 
		UIManager.Instance.DeployRobot ();
    }

    public void build()
    {
        state = GameState.Build;
        if (city != null)
        {
			city.SetActive (false);
        }
		UIManager.Instance.UndeployRobot ();
    }

    void Awake()
    {
		//state = GameState.Start;

		audioSource = this.GetComponent<AudioSource> ();
		soundManager = SoundManager.Instance;
		robot = Robot.Instance;
		audioSource.clip = soundManager.buildBGM;
		audioSource.loop = true;
		audioSource.Play ();

		if (state == GameState.Start) {
			HideRoom ();

		}
    }

	// Use this for initialization
	void Start () {
		//Invoke ("StartGame", 5f);
	}
	
	// Update is called once per frame
	void Update () {
		KeyControls ();
	}

	public void StartGame() {
		state = GameState.SelectBase;
		RobotBases.SetActive (true);
		UIManager.Instance.StartGame ();
	}

	public void SelectBase(int robotBase) {
		switch (robotBase) {
		case(1):
			selectedBase = RobotBases.GetComponent<SelectRobotBase> ().BaseOne;
			break;
		case(2):
			selectedBase = RobotBases.GetComponent<SelectRobotBase> ().BaseTwo;
			break;
		}

		SpawnRobotBase (selectedBase);
		ShowRoom ();
	
	}


	private void HideRoom() {
		//RenderSettings.ambientIntensity = 0f;
		foreach (GameObject component in RoomComponents) {
			component.SetActive (false);
		}
		Deployer.Instance.gameObject.GetComponent<SphereCollider> ().enabled = false;
	}



	private void ShowRoom() {
		state = GameState.Build;
		RobotBases.SetActive (false);
		foreach (GameObject component in RoomComponents) {
			component.SetActive (true);
		}
		Deployer.Instance.gameObject.GetComponent<SphereCollider> ().enabled = true;
		//RenderSettings.ambientIntensity = 1f;
	}



	private void SpawnRobotBase(GameObject robotBase) {
		Part robotPart = robotBase.GetComponent<Part> ();
		if (robotPart) {
			GameObject prefab = Resources.Load("Prefabs/" + robotPart.name) as GameObject;
			GameObject sObj = Object.Instantiate(prefab, new Vector3 (0f, robotBase.transform.localScale.y/2f, 0f), Quaternion.identity) as GameObject;

			sObj.transform.parent = robot.transform;
			Part spawnedPart = sObj.GetComponent<Part>();
			spawnedPart.template = false;
			spawnedPart.setState (Part.State.Placed);
			return;

		}
	}

	private void KeyControls() {
		if (Input.GetKeyDown (KeyCode.I)) {
			StartGame ();
		}

	}
}

