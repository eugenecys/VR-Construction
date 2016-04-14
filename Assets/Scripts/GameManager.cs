using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    public enum GameState
    {
        Build,
        Play
    }

    public GameState state;
    public GameObject city;

    public void play()
    {
        state = GameState.Play;
        city.SetActive(true);
        //GameObject prefab = Resources.Load("Prefabs/City") as GameObject;
        //city = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public void build()
    {
        state = GameState.Build;
        //if (city != null)
        //{
        //    Destroy(city);
        //}
        city.SetActive(false);
    }

    void Awake()
    {
        city.SetActive(false);
        state = GameState.Build;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
