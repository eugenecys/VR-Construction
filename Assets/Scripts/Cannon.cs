using UnityEngine;
using System.Collections;

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

    public override void fire()
    {
        GameObject sObj = Object.Instantiate(ammo, dirCoordinator.transform.position, dirCoordinator.transform.rotation) as GameObject;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        rb.velocity = ammoVelocity * dirCoordinator.transform.forward;
        eventManager.addEvent(() => Destroy(sObj), 2f, true);
    }
}
