﻿using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(AudioSource))]

public class LaserControl : Weapon {
    public float lineLen;
    public float duration;
    public GameObject laser;
    private MeshRenderer mesh;

    private AudioSource audioSource;
    private SoundManager soundManager;

    void Awake()
    {

        soundManager = SoundManager.Instance;
        audioSource = GetComponent<AudioSource>();
        mesh = laser.GetComponent<MeshRenderer>();
        audioSource.clip = soundManager.laserSound;
    }

    // Use this for initialization
    void Start () {
        mesh.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    IEnumerator FireLaser() {
        mesh.enabled = true;

        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, lineLen))
        {
            if (hit.transform.tag == "building")
            {
                hit.transform.gameObject.SendMessage("GiveAttack");
            }
        }

        yield return new WaitForSeconds(duration);

        mesh.enabled = false;
    }

    public override void trigger()
    {
        StopCoroutine(FireLaser());
        StartCoroutine(FireLaser());
        audioSource.Play();
        //throw new NotImplementedException();
    }

    public override void joystick(Vector2 coordinates)
    {
    }
}
