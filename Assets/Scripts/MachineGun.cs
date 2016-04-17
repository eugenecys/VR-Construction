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
        audioSource.Play();
        //throw new NotImplementedException();
    }

    
}
