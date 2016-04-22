using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager :  Singleton<ScoreManager>{

    private static int score = 0;

	private int endScore = 0;

    public int _score{
        get { return score; }
        set { score = value; }
    }

	private int[] highScores = new int[10];

    public void AddScore() {
		score += 100;
    }

	// Use this for initialization
	void Start () {
		GetPlayerPrefs ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void UpdateHighScores() {
		int rank = 0;
		// update the rank itself, and move all ranks >= rank down by 1. 
		for (int i= 0; i < 10; i++) {
			if (endScore >= highScores [i]) {
				//  move all scores down by 1
				for (int j = 9; j > i; j--) {
					highScores [j] = highScores [j - 1];
				}
				highScores [i] = endScore;
				break;
			} else {
				continue;
			}
		}

		for (int i = 0; i < 10; i++) {
			PlayerPrefs.SetInt ((i + 1).ToString (), highScores [i] );
		}
	}

	void GetPlayerPrefs() {
		if (!PlayerPrefs.HasKey ("1")) {
			for (int i = 0; i < 10; i++) {
				PlayerPrefs.SetInt ((i + 1).ToString (), 0);
			}
		}
		for (int i = 0; i < 10; i++) {
			highScores [i] = PlayerPrefs.GetInt ((i + 1).ToString ());
		}
	}


	public void SetEndScore(int finalScore) {
		endScore = finalScore;
		UpdateHighScores ();
	}
}
