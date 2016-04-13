using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    public enum GameState
    {
        Build,
        Play
    }

    public GameState state;
    private GameObject city;

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
        state = GameState.Build;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
