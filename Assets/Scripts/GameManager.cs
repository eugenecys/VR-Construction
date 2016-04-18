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

    public GameState state;
    public GameObject city;

	public Light roomLight; 

	public GameObject RobotBases;

	public GameObject selectedBase;

	public GameObject Deployer; 

    public void play()
    {
        state = GameState.Play;
		city.SetActive (true); 
    }

    public void build()
    {
        state = GameState.Build;
        if (city != null)
        {
			city.SetActive (false);
        }
    }

    void Awake()
    {
		//state = GameState.Start;

		audioSource = this.GetComponent<AudioSource> ();
		soundManager = SoundManager.Instance;

		if (state == GameState.Start) {
			Deployer.GetComponent<SphereCollider> ().enabled = false;
			roomLight.enabled = false;
			audioSource.clip = soundManager.buildBGM;
			audioSource.loop = true;

			audioSource.Play ();
		}
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame() {
		state = GameState.SelectBase;
		RobotBases.SetActive (true);
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
		state = GameState.Build;
		RobotBases.SetActive (false);
		roomLight.enabled = true;
		Deployer.GetComponent<SphereCollider>().enabled = true;

		SpawnRobotBase (selectedBase);

	}


	private void SpawnRobotBase(GameObject robotBase) {
		Part robotPart = robotBase.GetComponent<Part> ();
		if (robotPart) {
			GameObject prefab = Resources.Load("Prefabs/" + robotPart.name) as GameObject;
			GameObject sObj = Object.Instantiate(prefab, new Vector3 (0f, robotBase.transform.localScale.y/2f, 0f), Quaternion.identity) as GameObject;

			Part spawnedPart = sObj.GetComponent<Part>();
			spawnedPart.template = false;
			spawnedPart.setState (Part.State.Placed);
			return;

		}
	}
}
