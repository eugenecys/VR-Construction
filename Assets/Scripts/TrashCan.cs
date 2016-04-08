using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class TrashCan : Singleton<TrashCan> {
    
    Rigidbody rb;
    Part markedPart;

    void OnTriggerStay(Collider other)
    {
        Part part = other.GetComponentInParent<Part>();
        if (part != null)
        {
            markedPart = part;
            markedPart.markForDelete();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (markedPart != null)
        {
            markedPart.unmarkForDelete();
            markedPart = null;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
