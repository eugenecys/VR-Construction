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
    }

    public void build()
    {
        state = GameState.Build;
        city.SetActive(false);
    }

    void Awake()
    {
        state = GameState.Build;
        city.SetActive(false);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
