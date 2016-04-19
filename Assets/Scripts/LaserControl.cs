using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(AudioSource))]

public class LaserControl : Weapon {
    private AudioSource audioSource;
    private SoundManager soundManager;

    public float laserLen;
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
	    
	}
    
    void FireLaser() {
		var prefab = Instantiate (laser, transform.position, Quaternion.identity) as GameObject;
		var blast = prefab.GetComponent<SuperBlast> ();
        blast.blastSize = laserLen;
        blast.Launch(transform.position + transform.forward * laserLen);
    }

    public override void trigger()
    {
        FireLaser();
        audioSource.Play();
        //throw new NotImplementedException();
    }

    public override void joystick(Vector2 coordinates)
    {
    }
}
