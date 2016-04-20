using UnityEngine;
using System.Collections;

public class ScoreManager :  Singleton<ScoreManager>{

    private static int score = 0;

    public int _score{
        get { return score; }
        set { score = value; }
    }

    public void AddScore() {
        ++score;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
