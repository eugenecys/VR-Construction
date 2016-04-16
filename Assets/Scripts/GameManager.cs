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

    public GameState state;
    private GameObject city;

	public Light roomLight; 

	public GameObject RobotBases;

	public GameObject selectedBase;

    public void play()
    {
        state = GameState.Play;

        GameObject prefab = Resources.Load("Prefabs/City") as GameObject;
        city = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public void build()
    {
        state = GameState.Build;
        if (city != null)
        {
            Destroy(city);
        }
    }

    void Awake()
    {
		//state = GameState.Start;
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
		roomLight.gameObject.SetActive (true);
		SpawnRobotBase (selectedBase);
	}


	private void SpawnRobotBase(GameObject robotBase) {
		// set all children to be active
		for (int i = 0; i < robotBase.transform.childCount; i++) {
			robotBase.transform.GetChild (i).gameObject.SetActive (true);
		}
		Part part = robotBase.GetComponent<Part>();
		List<Part> parts = part.getConnectedParts();

		foreach(Part child in parts)
		{
			child.template = false;
		}
		GameObject.Instantiate (robotBase, new Vector3 (0f, robotBase.transform.localScale.y, 0f), Quaternion.identity);
	}
}
