using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIKeyboard : Singleton<UIKeyboard> {

	public int maxNameSize = 5;
	private char[] name;

	private int currentIndex = 0;

	public Text playerName; 
	// Use this for initialization
	void Start () {
		name = new char[maxNameSize];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void UpdateName(char letter) {
		if (currentIndex < maxNameSize) {
			name [currentIndex] = letter;
			currentIndex++;
			playerName.text = new string (name);
		}
	}

	public void SelectLetter(char letter) {
		UpdateName (letter);
	}

	public void BackSpace() {
		if (currentIndex >= 0) {
			name [currentIndex] = ' ';
			playerName.text = new string (name);
			if (currentIndex > 0) {
				currentIndex--;
			}
		}
	}

	public void SubmitName() {
		// set this gameobject ot inactive
		// update high scores with name and score and show them
		GameManager.Instance.SubmitHighScore (new string (name));
	}
}
