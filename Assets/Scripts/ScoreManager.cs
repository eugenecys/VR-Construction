using UnityEngine;
using System.Collections;

public class ScoreManager :  Singleton<ScoreManager>{

    private int score = 0;

    public void SetScore(int s) {
        score = s;
    }

    public int GetScore() {
        int tmp = score;
        return tmp;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
