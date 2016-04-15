using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.UI;


public class LaserPointer : MonoBehaviour {

	public bool active = true;
	public Color color;
	public float thickness = 0.002f;
	public GameObject holder;
	public GameObject pointer;
	bool isActive = false;
	public bool addRigidBody = false;
	public Transform reference;
	public event PointerEventHandler PointerIn;
	public event PointerEventHandler PointerOut;

	Transform previousContact = null;


	// Use this for initialization
	void Start () {
		holder = new GameObject();
		holder.transform.parent = this.transform;
		holder.transform.localPosition = Vector3.zero;

		pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pointer.transform.parent = holder.transform;
		pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
		pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
		BoxCollider collider = pointer.GetComponent<BoxCollider>();
		if (addRigidBody)
		{
			if (collider)
			{
				collider.isTrigger = true;
			}
			Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
			rigidBody.isKinematic = true;
		}
		else
		{
			if(collider)
			{
				UnityEngine.Object.Destroy(collider);
			}
		}
		Material newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", color);
		pointer.GetComponent<MeshRenderer>().material = newMaterial;


		PointerIn += new PointerEventHandler (HittingSomething);
		PointerOut += new PointerEventHandler (HittingNothing);
	}
	
	// Update is called once per frame
	void Update () {
		CastLaser ();
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
		if (e.target.tag == "StartGame" ) {
			Debug.Log ("gets here");
		}
		Debug.Log ("gets here0");
	}

	private void HittingNothing(object sender, PointerEventArgs e) {
		Debug.Log ("gets here1");
	}

	void CastLaser ()
	{
		if (!isActive)
		{
			isActive = true;
			this.transform.GetChild(0).gameObject.SetActive(true);
		}

		float dist = 100f;

		SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();

		Ray raycast = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit, dist);

		if(previousContact && previousContact != hit.transform)
		{
			PointerEventArgs args = new PointerEventArgs();
			if (controller != null)
			{
				args.controllerIndex = controller.controllerIndex;
			}
			args.distance = 0f;
			args.flags = 0;
			args.target = previousContact;
			OnPointerOut(args);
			previousContact = null;
		}
		if(bHit && previousContact != hit.transform)
		{
			PointerEventArgs argsIn = new PointerEventArgs();
			if (controller != null)
			{
				argsIn.controllerIndex = controller.controllerIndex;
			}
			argsIn.distance = hit.distance;
			argsIn.flags = 0;
			argsIn.target = hit.transform;
			OnPointerIn(argsIn);
			previousContact = hit.transform;
		}
		if(!bHit)
		{
			previousContact = null;
		}
		if (bHit && hit.distance < 100f)
		{
			dist = hit.distance;
		}

		if (controller != null && controller.triggerPressed)
		{
			pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
		}
		else
		{
			pointer.transform.localScale = new Vector3(thickness, thickness, dist);
		}
		pointer.transform.localPosition = new Vector3(0f, 0f, dist/2f);
	}
}

