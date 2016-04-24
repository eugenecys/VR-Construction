using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]

public class MachineGun : Weapon {
    private Random rand;
    public GameObject ammo;
    public float ammoVelocity;
    public float scatterRadius = 0.5f;
    public float scatterAngle = 10f;

    private AudioSource audioSource;
    private SoundManager soundManager;

    void Awake()
    {
        eventManager = EventManager.Instance;
        soundManager = SoundManager.Instance;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundManager.cannonSound;
        rand = new Random();
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
        float dx = Mathf.Cos(radialAngle * Mathf.PI / 180);
        float dy = Mathf.Sin(radialAngle * Mathf.PI / 180);
        float dist = rand.Next(0, 100) * scatterRadius / 100;
        Vector3 scatter = new Vector3(dx * dist, dy * dist, 0);
        GameObject sObj = Instantiate(ammo, transform.position + scatter, Quaternion.identity) as GameObject;
        sObj.transform.localScale *= multi;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        Vector3 trajectory = new Vector3(Mathf.Tan(scatterAngle * Mathf.PI / 180) * dx, Mathf.Tan(scatterAngle * Mathf.PI / 180) * dy, 1);
        rb.velocity = ammoVelocity * trajectory;
        eventManager.addEvent(() => Destroy(sObj), 2f, true);
        audioSource.Play();
    }

    protected override void AmmoScale(float scale)
    {
        ammo.transform.localScale *= scale;
    }

}
