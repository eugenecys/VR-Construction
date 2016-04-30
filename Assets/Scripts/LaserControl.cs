using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(AudioSource))]

public class LaserControl : Weapon {
    private AudioSource audioSource;
    private SoundManager soundManager;

    public float laserLen;
	public float laserRadius;
	public GameObject laser;

    void Awake()
    {

        soundManager = SoundManager.Instance;
        audioSource = GetComponent<AudioSource>();
        //mesh = laser.GetComponent<MeshRenderer>();
        audioSource.clip = soundManager.laserSound;
    }

    // Use this for initialization
    void Start () {
        //mesh.enabled = false;
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
    
    void FireLaser() {
		
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
    }

    protected override void Fire()
    {
		cooldownWeapon ();
        var prefab = Instantiate(laser, transform.position, Quaternion.identity) as GameObject;
        var blast = prefab.GetComponent<SuperBlast>();
        prefab.transform.parent = transform;
        prefab.transform.localPosition = Vector3.zero;
        blast.blastSize = laserRadius;
        blast.Launch(transform.position + transform.forward * laserLen);
        audioSource.Play();
    }

    protected override void AmmoScale(float scale)
    {
        laserLen *= scale;
        laserRadius *= scale;
        laser.transform.localScale *= scale;
    }
}
