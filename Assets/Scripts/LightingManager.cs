﻿using UnityEngine;
using System.Collections;

public class LightingManager : Singleton<LightingManager> {

	public Light baseLight1;
	public Light baseLight2;
	public Light[] roomLights;

	private AudioSource audioSource;

	// Use this for initialization
	void Awake () {
		foreach (Light light in roomLights) {
			light.enabled = false;
		}
		baseLight1.enabled = false;
		baseLight2.enabled = false;
	}

	// sounds all go into start
	void Start() {
		
		audioSource = this.GetComponent<AudioSource> ();
		audioSource.clip = SoundManager.Instance.lightOnSound;
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void StartGame() {
		baseLight1.enabled = true;
		baseLight1.intensity = 0f;
		baseLight2.enabled = true;
		baseLight2.intensity = 0f;
		StartCoroutine(EnableLightingSlowly (baseLight1));
		StartCoroutine(EnableLightingSlowly (baseLight2));
	}

	public void SelectedBase() {
		audioSource.Play ();
		StopAllCoroutines ();
		baseLight1.enabled = false;
		baseLight2.enabled = false;
		foreach (Light light in roomLights) {
			light.enabled = true;
			//light.intensity = 0f;
			//StartCoroutine(EnableLightingSlowly (light));
		}

	}

	IEnumerator EnableLightingSlowly(Light light) {
		while (light.intensity < 1f) {
			light.intensity += Time.deltaTime/2f;
			if (light.intensity >= 1f) {
				StopCoroutine ("EnableLightlySlowly");
			}
			yield return null;
		}
		yield return null;
	}


}
