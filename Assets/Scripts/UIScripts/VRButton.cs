using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VRButton : MonoBehaviour, Interactable
{
	public Button.ButtonClickedEvent btnFunction;
	public Material selected;

	private bool highlighted = false;
	private Image btnImage;
	// Use this for initialization
	void Awake ()
	{
		btnImage = this.transform.parent.GetComponent<Image> ();
		unhighlight ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
	{
		if (btnImage)
			btnImage.material = selected;
		highlighted = true;
	}

	public void unhighlight ()
	{
		if (btnImage)
			btnImage.material = null;
		highlighted = false;
	}

    public void online ()
    {

    }

    public void offline()
    {

    }

}
