using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager :  Singleton<ScoreManager>{

    private static int score = 0;

	private int endScore = 0;
	private  string endName = "Roy";

    public int _score{
        get { return score; }
        set { score = value; }
    }

	private int[] highScores = new int[10];
	private string[] highScoreNames = new string[10];

    public void AddScore() {
		score += 100;
    }

	// Use this for initialization
	void Awake () {
		GetPlayerPrefs ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void UpdateHighScores() {
		
		// update the rank itself, and move all ranks >= rank down by 1. 
		for (int i= 0; i < 10; i++) {
			if (endScore >= highScores [i]) {
				//  move all scores down by 1
				for (int j = 9; j > i; j--) {
					highScores [j] = highScores [j - 1];
					highScoreNames [j] = highScoreNames [j - 1];
				}
				highScores [i] = endScore;
				highScoreNames [i] = endName;
				break;
			} else {
				continue;
			}
		}
			
		for (int i = 0; i < 10; i++) {
			PlayerPrefs.SetInt ((i + 1).ToString (), highScores [i]);
			PlayerPrefs.SetString ((i + 1).ToString () + "Name", highScoreNames [i]);
		}
	
	}
		

	private bool isHighScore() {
		bool updated = false;
		// update the rank itself, and move all ranks >= rank down by 1. 
		for (int i = 0; i < 10; i++) {
			if (endScore >= highScores [i]) {
				return true;
			}
		}
		return false; 
	}

	void GetPlayerPrefs() {
		if (!PlayerPrefs.HasKey ("1")) {
			for (int i = 0; i < 10; i++) {
				PlayerPrefs.SetInt ((i + 1).ToString (), 0);
			}
			for (int i = 0; i < 10; i++) {
				Debug.Log ("gets here");
				PlayerPrefs.SetString ((i + 1).ToString () + "Name", "_____");
			}
		}

		// set the arrays for score and their names from playerprefs
		for (int i = 0; i < 10; i++) {
			highScores [i] = PlayerPrefs.GetInt ((i + 1).ToString ());
		}

		for (int i = 0; i < 10; i++) {
			highScoreNames[i] = PlayerPrefs.GetString ((i + 1).ToString () + "Name");
		}
	}


	public bool SetEndScore(int finalScore) {
		endScore = finalScore;
		return isHighScore ();
	}


	public void SetEndName(string playername) {
		endName = playername;
		UpdateHighScores ();
	}
}
