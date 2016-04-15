using UnityEngine;
using System.Collections;
using System;

public class MachineGun : Weapon,Controllable{
    public GameObject ammo;
    public float ammoVelocity;
        
    void Awake()
    {
        eventManager = EventManager.Instance;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void trigger()
    {
        GameObject sObj = Instantiate(ammo, transform.position, Quaternion.identity) as GameObject;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        rb.velocity = ammoVelocity * transform.forward;
        eventManager.addEvent(() => Destroy(sObj), 2f, true);
        //throw new NotImplementedException();
    }

    public override void joystick(Vector2 coordinates)
    {
        throw new NotImplementedException();
    }
}
