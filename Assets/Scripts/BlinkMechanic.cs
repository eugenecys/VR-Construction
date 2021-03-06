﻿using UnityEngine;
using System.Collections;

public class BlinkMechanic : MonoBehaviour
{

	public Transform trackingSpace;
	public Transform head; 

	// variables for shift teleporting
	private float shiftSpeed = 5f;
	private float shiftStopDist = 0.1f;

	private Vector3 oldCenter = Vector3.zero;
	private Vector3 newCenter = Vector3.zero;
	private Transform baseBody;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void gripDown ()
	{
		baseBody = Robot.Instance.gameObject.GetComponentInChildren<Base>().gameObject.transform;
		BlinkTo (baseBody.position);
	}

	private void BlinkTo (Vector3 robotPos)
	{

		oldCenter = trackingSpace.transform.position;
		newCenter = robotPos;
		StartCoroutine (ShiftTeleport ());
	}

	IEnumerator ShiftTeleport ()
	{
		while (true) {
			Vector3 dir = newCenter - trackingSpace.transform.position;
			if (dir.magnitude > shiftStopDist) {
				// still moving to our destination
				trackingSpace.transform.Translate (dir * shiftSpeed * Time.deltaTime, Space.World);
			} else {
				// we have reached our destination 
				StopCoroutine (ShiftTeleport ());
			}
			yield return null;
		}

	}

	private Vector3 GetNewCenterOfTrackingSpace(Vector3 pos) {
		Vector3 newCenter = pos;

		Vector3 temp = Vector3.zero;
		temp.x = (oldCenter.x - head.position.x);
		temp.z = (oldCenter.z - head.position.z);

		newCenter = newCenter + temp;
		return newCenter;
	}
		
}
