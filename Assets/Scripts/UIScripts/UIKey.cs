using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIKey : MonoBehaviour {

	private char[] letters;
	public bool isBackspace = false;
	public bool isEnter = false;
	// Use this for initialization
	void Start () {
		letters = this.transform.parent.GetComponentInChildren<Text> ().text.ToCharArray ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void keySelected() {
		if (!isBackspace && !isEnter) {
			UIKeyboard.Instance.SelectLetter (letters [0]);
		} else if (isBackspace) {
			UIKeyboard.Instance.BackSpace ();
		} else if (isEnter) {
			UIKeyboard.Instance.SubmitName ();
		}

	}
}
