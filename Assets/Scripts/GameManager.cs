using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{

	public bool debug = true;

	public enum GameState
	{
		Build,
		Play,
		Start,
		SelectBase,
		End
	}

	private AudioSource musicSource;
	private AudioSource dialogueSource;
	private SoundManager soundManager;
	private Robot robot;

	public GameState state;
	public GameObject city;
	public Transform trackingSpace;

	public GameObject RobotBases;

	public GameObject selectedBase;

	public GameObject[] RoomComponents;


	public void play ()
	{
		state = GameState.Play;
		city.SetActive (true); 
		UIManager.Instance.DeployRobot ();
		TimeManager.Instance.StartDeployCountdown ();
		PlayDialogue (soundManager.cityDialogue);
	}

	public void build ()
	{
		state = GameState.Build;
		if (city != null) {
			city.SetActive (false);
		}
		UIManager.Instance.UndeployRobot ();
	}


	// Use this for initialization
	void Awake ()
	{
		//state = GameState.Start;
		AudioSource[] sources = this.GetComponents<AudioSource> ();
		musicSource = sources [0];
		dialogueSource = sources [1];

		soundManager = SoundManager.Instance;
		robot = Robot.Instance;

		if (state == GameState.Start) {
			HideRoom ();
		}
	}

	// sounds all go into start
	void Start ()
	{
		PlayMusic (soundManager.buildBGM);
		PlayDialogue (soundManager.startDialogue);
		Invoke ("StartGame", 16f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void StartGame ()
	{
		if (state == GameState.Start) {
			state = GameState.SelectBase;
			RobotBases.SetActive (true);
			LightingManager.Instance.StartGame ();

			UIManager.Instance.StartGame ();
			StopDialogue ();
			PlayDialogue (soundManager.selectBaseDialogue);
		}
	}

	public void SelectBase (int robotBase)
	{
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
		LightingManager.Instance.SelectedBase ();

		StopDialogue ();
		PlayDialogue (soundManager.constructionDialogue);
		Invoke ("ReadyDeployButton", 20.0f);
	}


	private void HideRoom ()
	{
		foreach (GameObject component in RoomComponents) {
			component.SetActive (false);
		}
		Deployer.Instance.gameObject.GetComponent<SphereCollider> ().enabled = false;
	}



	private void ShowRoom ()
	{
		state = GameState.Build;
		RobotBases.SetActive (false);
		foreach (GameObject component in RoomComponents) {
			component.SetActive (true);
		}
		if (debug) {
			Deployer.Instance.gameObject.GetComponent<SphereCollider> ().enabled = true;
		}

	}



	public void SpawnRobotBase (GameObject robotBase)
	{
		Part robotPart = robotBase.GetComponent<Part> ();
		if (robotPart) {
			GameObject prefab = Resources.Load ("Prefabs/" + robotPart.name) as GameObject;
			GameObject sObj = Object.Instantiate (prefab, new Vector3 (0f, robotBase.transform.localScale.y / 2f, 0f), robotBase.transform.rotation) as GameObject;

			sObj.transform.parent = robot.transform;
			Part spawnedPart = sObj.GetComponent<Part> ();
			spawnedPart.template = false;
			spawnedPart.setState (Part.State.Placed);
			return;

		}
	}

	public void EndGame ()
	{
		state = GameState.End;
		Building[] buildings = city.GetComponentsInChildren<Building> ();
		foreach (Building building in buildings) {
			if (Vector3.Distance (trackingSpace.position, building.transform.position) < 10.0f) {
				building.SelfDestruct ();
			}
		}
		if (ScoreManager.Instance.SetEndScore (ScoreManager.Instance._score)) {
			// then we have a new high score
			UIManager.Instance.EndGame (ScoreManager.Instance._score, true);
		} else {
			UIManager.Instance.EndGame (ScoreManager.Instance._score, false);
		}

	}

	public void SubmitHighScore (string playerName)
	{
		ScoreManager.Instance.SetEndName (playerName);
		UIManager.Instance.NameSubmitted ();
	}

	public void PlayDialogue (AudioClip clip)
	{
		dialogueSource.clip = clip;
		dialogueSource.Play ();
	}

	public void StopDialogue ()
	{
		dialogueSource.Stop ();
	}

	public void PlayMusic (AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.loop = true;
		musicSource.Play ();
	}

	public void StopMusic ()
	{
		musicSource.Stop ();
	}

	private void ReadyDeployButton ()
	{
		dialogueSource.clip = soundManager.deployDialogue;
		dialogueSource.Play ();
		Deployer.Instance.gameObject.GetComponent<SphereCollider> ().enabled = true;
	}
}

