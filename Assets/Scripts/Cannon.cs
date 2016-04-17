using UnityEngine;
using System.Collections;
using System;

public class Cannon : Weapon {
    public GameObject ammo;
    public float ammoVelocity;
    public GameObject dirCoordinator;
    // Use this for initialization

    void Awake()
    {
        eventManager = EventManager.Instance;
    }
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public override void trigger()
    {
        GameObject sObj = Instantiate(ammo, dirCoordinator.transform.position, dirCoordinator.transform.rotation) as GameObject;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        rb.velocity = ammoVelocity * dirCoordinator.transform.forward;
        eventManager.addEvent(() => Destroy(sObj), 2f, true);
        //throw new NotImplementedException();
    }

    public override void joystick(Vector2 coordinates)
    {
        //throw new NotImplementedException();
    }
}
