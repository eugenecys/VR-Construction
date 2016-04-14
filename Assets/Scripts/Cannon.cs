using UnityEngine;
using System.Collections;

public class Cannon : Weapon {
    public GameObject ammo;
    public float ammoVelocity;
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
        GameObject sObj = Object.Instantiate(ammo, transform.position, Quaternion.identity) as GameObject;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        rb.velocity = ammoVelocity * transform.forward;
        eventManager.addEvent(() => Destroy(sObj), 2f, true);
    }
}
