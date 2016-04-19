using UnityEngine;
using System.Collections;

public class SelectRobotBase : MonoBehaviour {

	public GameObject BaseOne;
	public GameObject BaseTwo;
	public float rotationSpeed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		RotateBases ();
	}


	private void RotateBases() {
		BaseOne.transform.RotateAroundLocal (Vector3.up, Time.deltaTime * rotationSpeed);
		BaseTwo.transform.RotateAroundLocal (Vector3.up, Time.deltaTime * rotationSpeed);
	}
}
