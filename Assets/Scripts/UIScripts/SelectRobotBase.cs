using UnityEngine;
using System.Collections;

public class SelectRobotBase : MonoBehaviour {

	public GameObject BaseOne;
	public GameObject BaseTwo;
    public GameObject BaseOnePlaceholder;
    public GameObject BaseTwoPlaceholder;
    public float rotationSpeed = 15f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		RotateBases ();
	}


	private void RotateBases() {
        BaseOnePlaceholder.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
		BaseTwoPlaceholder.transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
	}
}
