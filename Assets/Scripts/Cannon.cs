using UnityEngine;
using System.Collections;
using System;
[RequireComponent(typeof(AudioSource))]
public class Cannon : Weapon {
    public GameObject ammo;
    public float ammoVelocity;
    public GameObject dirCoordinator;
    private AudioSource audioSource;
    private SoundManager soundManager;
    // Use this for initialization

    void Awake()
    {
        eventManager = EventManager.Instance;
        soundManager = SoundManager.Instance;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundManager.cannonSound;
    }
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public override void trigger()
    {
        audioSource.Play();
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
