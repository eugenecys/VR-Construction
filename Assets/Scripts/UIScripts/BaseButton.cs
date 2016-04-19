using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour, Interactable {

	public Button.ButtonClickedEvent btnFunction;
	public Light baseLight; 
	private Color originalColor; 
	private bool highlighted = false;
	// Use this for initialization
	void Start () {
		originalColor = baseLight.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay (Collider col)
	{
		if (!highlighted) {
			highlight ();
		}
	}

	void OnTriggerExit (Collider col)
	{
		unhighlight ();
	}

	public void highlight ()
	{	baseLight.color = Color.blue;
		highlighted = true;
	}

	public void unhighlight ()
	{
		baseLight.color = originalColor;
		highlighted = false;
	}
}
	
