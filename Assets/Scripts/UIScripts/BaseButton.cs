using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour, Interactable {

	public Button.ButtonClickedEvent btnFunction;
	public Light baseLight; 
	private Color originalColor; 
	private bool highlighted = false;
	// Use this for initialization
	void Awake () {
		originalColor = baseLight.color;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerStay (Collider col)
	{
		if (!highlighted && (col.tag == "Laser")) {
			highlight ();
		}
	}

	void OnTriggerExit (Collider col)
	{	
		unhighlight ();
	}

	public void highlight ()

	{	baseLight.color = Color.red;
		highlighted = true;
	}

	public void unhighlight ()
	{
		baseLight.color = originalColor;
		highlighted = false;
	}

    public void online()
    {

    }

    public void offline()
    {

    }
}
	
