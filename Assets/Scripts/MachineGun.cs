using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]

public class MachineGun : Weapon {
    public GameObject ammo;
    public float ammoVelocity;

    private AudioSource audioSource;
    private SoundManager soundManager;

    void Awake()
    {
        eventManager = EventManager.Instance;
        soundManager = SoundManager.Instance;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundManager.cannonSound;
    }
    
    // Use this for initialization
    void Start () {
        fireCountDown = fireInteval;
	}
	
	// Update is called once per frame
	void Update () {
        if (isFiring) {
            Inteval();
        }
	}

    public override void trigger()
    {
        isFiring = true;
        //throw new NotImplementedException();
    }

    public override void triggerStop()
    {
        isFiring = false;
    }

    protected override void Fire()
    {
        GameObject sObj = Instantiate(ammo, transform.position, Quaternion.identity) as GameObject;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        rb.velocity = ammoVelocity * transform.forward;
        eventManager.addEvent(() => Destroy(sObj), 2f, true);
        audioSource.Play();
    }


}
