using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    private static ScoreManager singleton;

    public static ScoreManager getInstance() {
        if (singleton == null) {
            singleton = new ScoreManager();
        }
        return singleton;
    }

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
