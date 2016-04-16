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

	public event PointerEventHandler PointerIn;
	public event PointerEventHandler PointerOut;

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


		PointerIn += new PointerEventHandler (HittingSomething);
		PointerOut += new PointerEventHandler (HittingNothing);

		builder = this.gameObject.GetComponent<Builder> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			CastLaser ();
		}
	}


	public virtual void OnPointerIn(PointerEventArgs e)
	{
		if (PointerIn != null) 
			PointerIn(this, e);
	}

	public virtual void OnPointerOut(PointerEventArgs e)
	{
		if (PointerOut != null)
			PointerOut(this, e);
	}


	private void HittingSomething(object sender, PointerEventArgs e) {
		Debug.Log ("hitting something");
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
		


	void CastLaser ()
	{

		float dist = length;

		Ray raycast = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit, dist, laserMask);

		if(previousContact && previousContact != hit.transform)
		{
			PointerEventArgs args = new PointerEventArgs();
			args.distance = 0f;
			args.flags = 0;
			args.target = previousContact;
			OnPointerOut(args);
			previousContact = null;
		}

		if(bHit && previousContact != hit.transform)
		{
			PointerEventArgs argsIn = new PointerEventArgs();
			argsIn.distance = hit.distance;
			argsIn.flags = 0;
			argsIn.target = hit.transform;
			OnPointerIn(argsIn);
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

