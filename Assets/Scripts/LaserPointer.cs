using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public struct PointerData
{
	public uint controllerIndex;
	public uint flags;
	public float distance;
	public Transform target;
}


public class LaserPointer : MonoBehaviour {

	public bool active = true;

	public float thickness = 0.002f;
	public float length = 100f; 
	public GameObject holder;
	public GameObject pointer;
	public Material pointerMat;

	public LayerMask laserMask; 

	public Transform previousContact = null;

	private Builder builder; 

	private float raycastZOffset = -1.5f;

	void Awake() {
		builder = this.gameObject.GetComponent<Builder> ();
	}

	// Use this for initialization
	void Start () {
		holder = new GameObject();
		holder.transform.parent = this.transform;
		holder.transform.localPosition = new Vector3 (0f, 0f, raycastZOffset);
		holder.transform.localRotation = Quaternion.identity;

		pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pointer.transform.parent = holder.transform;
		pointer.transform.localScale = new Vector3(thickness, thickness, length);
		pointer.transform.localPosition = new Vector3(0f, 0f, length/2f );
		pointer.transform.localRotation = Quaternion.identity;
		pointer.transform.tag = "Laser";
		BoxCollider box = pointer.GetComponent<BoxCollider> ();
		box.isTrigger = true;
		//box.size = new Vector3 (5f, 5f, 1f);

		pointer.GetComponent<MeshRenderer>().material = pointerMat;

	}

	private bool building = false;
	// Update is called once per frame
	void FixedUpdate () {
		if (GameManager.Instance.state == GameManager.GameState.Play) {
			pointer.gameObject.SetActive (false); 
			if (building) {
				building = false;
				active = false;
				pointer.transform.localScale = new Vector3(thickness, thickness, 0f);
			}
		}
		else {
			if (!building) {
				building = true;
				active = true;
				pointer.transform.localScale = new Vector3(thickness, thickness, length);
			}
			pointer.gameObject.SetActive (true);
			if (active) {
				if (pointer.transform.localScale.z == 0f) {
					pointer.transform.localScale = new Vector3(thickness, thickness, length);
				}
				CastLaser ();
			} else {
				if (pointer.transform.localScale.z > 0f) {
					pointer.transform.localScale = new Vector3 (thickness, thickness, 0f);
				}
			}
		}


	}
		

	private void LaserEnter(PointerData data) {
		//Debug.Log ("hitting new thing");

		if (GameManager.Instance.state == GameManager.GameState.Build && builder) {
			builder.SetContactObject (data.target.gameObject);
			Interactable iObj = data.target.gameObject.GetComponent<Interactable>();
			if (iObj != null)
			{
				iObj.highlight();
                iObj.online();
			}
		}
	}

	private void LaserStay(PointerData data) {
		//Debug.Log ("hitting same thing");
		if (GameManager.Instance.state == GameManager.GameState.Build && builder) {
			builder.SetContactObject (data.target.gameObject);
			Interactable iObj = data.target.gameObject.GetComponent<Interactable> ();
			if (iObj != null) {
				iObj.highlight();
                iObj.online();
            }
		} 
	}

	private void LaserExit(PointerData data) {
		//Debug.Log ("hitting nothing");
		if (GameManager.Instance.state == GameManager.GameState.Build && builder) {
			if (previousContact) {
				Interactable iObj = previousContact.GetComponent<Interactable> ();
				if (iObj != null) {
					iObj.unhighlight ();
                    iObj.offline();
				}
			}
			builder.SetContactObject (null);
		}
	}

	void CastLaser ()
	{

		float dist = length;

		Ray raycast = new Ray(holder.transform.position, transform.forward);
		RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit, dist, laserMask);
		//Debug.DrawRay (holder.transform.position, transform.forward);

		if (previousContact && previousContact == hit.transform) {
			PointerData argsStay = new PointerData();
			argsStay.distance = hit.distance;
			argsStay.flags = 0;
			argsStay.target = hit.transform;
			LaserStay (argsStay);
			previousContact = hit.transform;
		}

		else if(previousContact && previousContact != hit.transform)
		{
			PointerData argsOut = new PointerData();
			argsOut.distance = 0f;
			argsOut.flags = 0;
			argsOut.target = previousContact;
			LaserExit(argsOut);
			previousContact = null;
		}

		else if(bHit && previousContact != hit.transform)
		{
			PointerData argsIn = new PointerData();
			argsIn.distance = hit.distance;
			argsIn.flags = 0;
			argsIn.target = hit.transform;
			LaserEnter(argsIn);
			previousContact = hit.transform;
		}


		// if haven't hit anything, set previous contact to null 
		if(!bHit)
		{
			previousContact = null;
		}

		// if we hit something we now reduce size of laser to match distance from us to what we hit
		if (bHit && hit.distance < dist)
		{
			dist = hit.distance;
		}

		pointer.transform.localScale = new Vector3(thickness, thickness, dist);
		
		pointer.transform.localPosition = new Vector3(0f, 0f, dist/2f);

	}

}

