using UnityEngine;
using System.Collections;

public class MachineGun : Weapon {
    public GameObject ammo;
    public float ammoVelocity;

    public override void trigger()
    {
        GameObject sObj = Object.Instantiate(ammo, transform.position, Quaternion.identity) as GameObject;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        rb.velocity = ammoVelocity * transform.forward;
        eventManager.addEvent(() => Destroy(sObj), 2f, true);
    }

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
}
