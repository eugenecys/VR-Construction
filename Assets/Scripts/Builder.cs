using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Builder : MonoBehaviour
{

	private Robot robot;
	private Segment activeComponent;
	private GameObject contactObject;
	private Vector3 spawnPosition;
	private Vector3 pullPosition;
	private GameObject currentPart = null;

	public Transform pullPoint;


	public Part[] childParts;
	Deployer deployer;
	public Builder other;

	private float pullSpeed = 15f;

	private ViveInputManager inputManager;
	private AudioSource audioSource;
	private SoundManager soundManager;

	private float refreshDelay = 0.2f;
	public bool triggered = false;

	private LaserPointer laser;


	public enum ColliderState
	{
		Far,
		Close
	}

	public ColliderState colliderState;

	public void setColliderState (ColliderState state)
	{
		colliderState = state;
		switch (state) {
		case ColliderState.Close:
                
			break;
		case ColliderState.Far:
			break;
		}
	}


	//Delete - keyboard code
	public void connectPart ()
	{
		activeComponent.parent.place ();
	}

	public void placeParts ()
	{
		foreach (Part part in childParts) {
			part.place ();
			if (part.placed) {
				part.transform.parent = robot.transform;
				part.resetPhysics ();
			} else if (part.placedInAir) {
				part.transform.parent = null;
				part.resetPhysics ();
			}
		}
	}

	public void deactivateRobot ()
	{
		robot.reset ();
	}

	public void activateRobot ()
	{
		robot.activate ();
	}

	public void deployRobot ()
	{
		robot.deploy ();
	}

	public void triggerRobot ()
	{
		robot.trigger ();
	}

	public void triggerRobotStop ()
	{
		robot.triggerStop ();
	}

	void Awake ()
	{
		robot = Robot.Instance;
		inputManager = ViveInputManager.Instance;
		deployer = Deployer.Instance;
		laser = this.GetComponent<LaserPointer> ();
		audioSource = this.GetComponent<AudioSource> ();
		soundManager = SoundManager.Instance;
	}

	// Use this for initialization
	void Start ()
	{
	
	}

	public void menu ()
	{
		DestroyRobot ();
		deployer.undeploy ();
		robot.reset ();
		GameManager.Instance.SpawnRobotBase (GameManager.Instance.selectedBase);
	}

	public void triggerUp ()
	{
		

		if (!triggered) {
			return;
		}
		triggered = false;
		childParts = GetComponentsInChildren<Part> ();
		if (childParts == null || childParts.Length == 0) {
			if (contactObject != null) {
				ScaleArrow scaleArrow = contactObject.GetComponent<ScaleArrow> ();
				if (scaleArrow == null) {

				} else {
					scaleArrow.stopDrag ();
				}
			}
		} else {
			List<Part> markedParts = new List<Part> ();
			foreach (Part part in childParts) {
				if (part.markedForDelete) {
					markedParts.Add (part);
				}
			}
			if (markedParts.Count > 0) {
				foreach (Part markedPart in markedParts) {
					Destroy (markedPart.gameObject);
				}
				audioSource.PlayOneShot (soundManager.trashSound);
			} else {
				placeParts ();
			}
		}
		contactObject = null;
		if (laser) {
			laser.active = true;
		}
	}

	public void triggerDown ()
	{
		
		if (triggered) {
			return;
		}
		triggered = true;
		if (contactObject != null) {
			ScaleArrow scaleArrow = contactObject.GetComponent<ScaleArrow> ();
			if (scaleArrow == null) {
				Part part = contactObject.GetComponentInParent<Part> ();
				if (part == null) {
					Deployer deployer = contactObject.GetComponent<Deployer> ();
					if (deployer == null) {

					} else {
						deployRobot ();
						deployer.deploy ();
						contactObject = null;
					}
				} else {
					if (part.template) {
						currentPart = SpawnComponent (part);
						PullComponent (currentPart);
						contactObject = null;
					} else {
						MoveComponent (part);
						currentPart = contactObject;
						PullComponent (contactObject);
						contactObject = null;
					}
				}
			} else {
				scaleArrow.followDrag (transform);
			}
			if (laser) {
				laser.active = false;
			}
		}
	}


	public void PullComponent (GameObject part)
	{
		currentPart = part;
		StartCoroutine (PullingComponent (part.GetComponent<Part>().distanceFromController));
	}


	IEnumerator PullingComponent (float distanceFromController)
	{
		UpdatePullPosition (distanceFromController);

		while (currentPart) {
			if (Vector3.Distance (pullPosition, currentPart.transform.position) > 0.1f && Vector3.Distance (pullPosition, currentPart.transform.position) < Mathf.Infinity ) {
				currentPart.transform.position = Vector3.Lerp (currentPart.transform.position, pullPosition, Time.deltaTime * pullSpeed);
				//currentPart.transform.Translate ((pullPosition - currentPart.transform.position) * Time.deltaTime * pullSpeed, Space.World);
			} else {
				currentPart = null;
				StopCoroutine ("PullingComponent");
			}
			yield return null;
		}
	}

	private void UpdatePullPosition(float distanceFromController) {
		pullPosition = pullPoint.position + pullPoint.forward.normalized * distanceFromController;
	}


	public void DestroyRobot ()
	{
		robot.destroy ();
	}


	public void MoveComponent (Part part)
	{
		part.disconnect ();
		part.transform.parent = this.transform;
		part.unplace ();
		audioSource.PlayOneShot (soundManager.pickupSound);
	}

	public GameObject SpawnComponent (Part part)
	{	
		spawnPosition = part.gameObject.transform.position;
		return SpawnComponent (part, spawnPosition);
	}

	void OnTriggerStay (Collider other)
	{
		contactObject = other.gameObject;
		Interactable iObj = contactObject.GetComponent<Interactable> ();
		if (iObj != null) {
			iObj.highlight ();
		}

	}

	void OnTriggerExit (Collider other)
	{
		if (contactObject != null) {
			Interactable iObj = contactObject.GetComponent<Interactable> ();
			if (iObj != null) {
				iObj.unhighlight ();
			}
		}
		contactObject = null;
	}
    
	// Update is called once per frame
	void Update ()
	{
		
	}

	public GameObject SpawnComponent (Part part, Vector3 position)
	{
		GameObject prefab = Resources.Load ("Prefabs/" + part.name) as GameObject;
		GameObject sObj = Object.Instantiate (prefab, position, prefab.transform.rotation) as GameObject;
		sObj.transform.parent = this.transform;
		Part spawnedPart = sObj.GetComponent<Part> ();
		spawnedPart.template = false;
		spawnedPart.evaluateState (false);
		audioSource.PlayOneShot (soundManager.pickupSound);

		if (!UIManager.Instance.pickedUpForFirstTime) {
			UIManager.Instance.ShowPickUpControls (false);
			UIManager.Instance.pickedUpForFirstTime = true;
		}

		return sObj;
	}


	public void SetContactObject (GameObject contact)
	{
		contactObject = contact;
	}
		
}
