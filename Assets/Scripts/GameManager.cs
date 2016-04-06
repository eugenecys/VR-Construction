using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    public enum GameState
    {
        Build,
        Play
    }

    public GameState state;

    public void play()
    {
        state = GameState.Play;
    }

    public void build()
    {
        state = GameState.Build;
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
