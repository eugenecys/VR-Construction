﻿using UnityEngine;
using System.Collections;

public class FollowController : MonoBehaviour
{

	public bool followController = true;
	public bool rotateWithController = true;
	public Transform ui;
	public Transform controller;
	public Transform lookingAt;
	public float distanceAboveController = 0f;

	public float followSpeed = 10f;
	public float lookUpAngle = 45f;
	private void Start ()
	{
		Vector3 controllerUp = (ui.position - controller.position).normalized;
		Vector3 localUp = ui.transform.up;

		// Allign the body's up axis with the centre of planet
		ui.rotation = Quaternion.FromToRotation (localUp, controllerUp) * ui.rotation;

	}


	private void Update ()
	{
		// If the UI should look at the controller set it's rotation to point from the UI to the controller
		if (rotateWithController) {
			Quaternion newRot = controller.rotation;
			Vector3 newEulur = newRot.eulerAngles;
			newEulur.x += lookUpAngle;
			newEulur.z = 0f;
			newRot.eulerAngles = newEulur;
			ui.rotation = newRot;
		}
		if (followController) {

			Vector3 targetPosition = controller.position + controller.up.normalized * distanceAboveController;
	
			targetPosition = Vector3.Lerp (ui.position, targetPosition, followSpeed * Time.deltaTime);
			ui.position = targetPosition;
		}
	}

}
