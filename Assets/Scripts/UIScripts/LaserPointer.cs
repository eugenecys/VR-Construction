using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.UI;


public class LaserPointer : MonoBehaviour {

	public bool active = true;
	public Color color;
	public float thickness = 0.002f;
	public float length = 100f; 
	public GameObject holder;
	public GameObject pointer;

	public LayerMask laserMask; 

	public event PointerEventHandler PointerEnter;
	public event PointerEventHandler PointerExit;
	public event PointerEventHandler PointerStay;

	public Transform previousContact = null;

	private Builder builder; 

	// Use this for initialization
	void Start () {
		holder = new GameObject();
		holder.transform.parent = this.transform;
		holder.transform.localPosition = Vector3.zero;
		holder.transform.localRotation = Quaternion.identity;

		pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pointer.transform.parent = holder.transform;
		pointer.transform.localScale = new Vector3(thickness, thickness, length);
		pointer.transform.localPosition = new Vector3(0f, 0f, length/2f);
		pointer.transform.localRotation = Quaternion.identity;
		pointer.transform.tag = "Laser";
		pointer.GetComponent<BoxCollider> ().isTrigger = true;

		Material newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", color);
		pointer.GetComponent<MeshRenderer>().material = newMaterial;


		PointerEnter += new PointerEventHandler (HittingNewThing);
		PointerExit += new PointerEventHandler (HittingNothing);
		PointerStay += new PointerEventHandler (HittingSameThing);

		builder = this.gameObject.GetComponent<Builder> ();
	}
	
	// Update is called once per frame
	void Update () {
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


	public virtual void OnPointerEnter(PointerEventArgs e)
	{
		if (PointerEnter != null) 
			PointerEnter(this, e);
	}

	public virtual void OnPointerExit(PointerEventArgs e)
	{
		if (PointerExit != null)
			PointerExit(this, e);
	}

	public virtual void OnPointerStay(PointerEventArgs e) 
	{
		if (PointerStay != null)
			PointerStay(this, e);
	}

	private void HittingNewThing(object sender, PointerEventArgs e) {
		Debug.Log ("hitting new thing");
		if (GameManager.Instance.state == GameManager.GameState.Build) {
			builder.SetContactObject (e.target.gameObject);
		}
	}

	private void HittingNothing(object sender, PointerEventArgs e) {
		Debug.Log ("hitting nothing");
		if (GameManager.Instance.state == GameManager.GameState.Build) {
			builder.SetContactObject (null);
		}
	}

	private void HittingSameThing(object sender, PointerEventArgs e) {
		//Debug.Log ("hitting same thing");
		if (GameManager.Instance.state == GameManager.GameState.Build) {
			builder.SetContactObject (e.target.gameObject);
		}
	}


	void CastLaser ()
	{

		float dist = length;

		Ray raycast = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit, dist, laserMask);

		if (previousContact && previousContact == hit.transform) {
			PointerEventArgs argsStay = new PointerEventArgs();
			argsStay.distance = hit.distance;
			argsStay.flags = 0;
			argsStay.target = hit.transform;
			OnPointerStay (argsStay);
			previousContact = hit.transform;
		}

		if(previousContact && previousContact != hit.transform)
		{
			PointerEventArgs argsOut = new PointerEventArgs();
			argsOut.distance = 0f;
			argsOut.flags = 0;
			argsOut.target = previousContact;
			OnPointerExit(argsOut);
			previousContact = null;
		}

		if(bHit && previousContact != hit.transform)
		{
			PointerEventArgs argsIn = new PointerEventArgs();
			argsIn.distance = hit.distance;
			argsIn.flags = 0;
			argsIn.target = hit.transform;
			OnPointerEnter(argsIn);
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

