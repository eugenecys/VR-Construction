using UnityEngine;
using System.Collections;
using System;
[RequireComponent(typeof(AudioSource))]
public class Cannon : Weapon {
    public GameObject ammo;
    public float ammoVelocity;
    public GameObject dirCoordinator;
	public ParticleSystem explosion;
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


		if (fireCountDown + coolDown < Time.time) {
			coolingDown = false;
		}

		canfire = triggerDown && !coolingDown;
		if (canfire) {
			Fire ();
		}
	}
    
    public override void trigger()
    {
        triggerDown = true;
        //throw new NotImplementedException();
    }

    public override void triggerStop()
    {
		triggerDown = false;
    }

    public override void joystick(Vector2 coordinates)
    {
        //throw new NotImplementedException();
    }

    protected override void Fire()
    {
		cooldownWeapon ();
        audioSource.Play();
        GameObject sObj = Instantiate(ammo, dirCoordinator.transform.position, dirCoordinator.transform.rotation) as GameObject;
        sObj.transform.localScale *= multi;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        rb.velocity = ammoVelocity * dirCoordinator.transform.forward;
        eventManager.addEvent(() => Destroy(sObj), 2f, true);
		explosion.Emit (300);

    }

    protected override void AmmoScale(float scale)
    {
        ammo.transform.localScale *= scale;
    }
}
