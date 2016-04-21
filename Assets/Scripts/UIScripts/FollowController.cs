using UnityEngine;
using System.Collections;

public class FollowController : MonoBehaviour {

	public bool followController = true;

	public Transform ui;
	public Transform controller;
	public Transform lookingAt;
	public float distanceAboveController = 0f;

	public float followSpeed = 10f;
	private void Start ()
	{
		Vector3 controllerUp = (ui.position -  controller.position).normalized;
		Vector3 localForward = ui.transform.forward;

		// Allign the body's up axis with the centre of planet
		ui.rotation = Quaternion.FromToRotation(localForward,controllerUp) * ui.rotation;

	}


	private void Update ()
	{
		// If the UI should look at the controller set it's rotation to point from the UI to the controller
		if (followController) {
			Quaternion newRot = Quaternion.LookRotation (ui.position - lookingAt.position);
			ui.rotation = newRot;
			Vector3 targetPosition = controller.position + controller.up.normalized * distanceAboveController;
	
			targetPosition = Vector3.Lerp (ui.position, targetPosition, followSpeed * Time.deltaTime);
			ui.position = targetPosition;
		} 

	}
}
