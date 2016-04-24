using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]

public class MachineGun : Weapon {
    private System.Random rand;
    public GameObject ammo;
    public float ammoVelocity;
    public float scatterRadius = 0.00f;
    public float scatterAngle = 1f;

    private AudioSource audioSource;
    private SoundManager soundManager;

    void Awake()
    {
        eventManager = EventManager.Instance;
        soundManager = SoundManager.Instance;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundManager.cannonSound;
        rand = new System.Random();
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
        int radialAngle = rand.Next(0, 360);
        float dx = Mathf.Cos(1.0f * radialAngle * Mathf.PI / 180);
		//Debug.Log (dx);
		float dy = Mathf.Sin(1.0f * radialAngle * Mathf.PI / 180);
		//Debug.Log (dy);
		float dist = rand.Next(0, 100) * scatterRadius * transform.localScale.x / 100;
		GameObject sObj = Instantiate(ammo, transform.position + transform.right * dx * dist + transform.up * dx * dist, Quaternion.identity) as GameObject;
        sObj.transform.localScale *= multi;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
		Vector3 trajectory = Mathf.Tan(scatterAngle * Mathf.PI / 180) * dx * transform.right + 
			Mathf.Tan(scatterAngle * Mathf.PI / 180) * dy * transform.up + 
			transform.forward;
		rb.velocity = ammoVelocity * trajectory;
        audioSource.Play();
    }

    protected override void AmmoScale(float scale)
    {
        ammo.transform.localScale *= scale;
    }

}
