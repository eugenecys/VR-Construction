﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Robot : Singleton<Robot>, Controllable {

	Part[] parts;
	Weapon[] weapons;
	public enum State
	{
		Inactive,
		Active,
		Deployed
	}

	public State state;

	public int strongerPowerLevel = 1500;
	public int weakerPowerLevel = 1000;
	public int maxPowerLevel = 1000;
	public int currentPowerLevel = 1000;

	public void destroy()
	{
		foreach (Transform child in transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

	private bool fired = false;
	public void trigger()
	{
		foreach (Part part in parts)
		{
			if (part.controllable)
			{
				part.trigger();
				if (!fired) {
					fired = true;
				} else if (moved) {
					GameManager.Instance.tutorialOver = true;
				}
			}
		}
	}

	public void triggerStop()
	{
		foreach (Part part in parts)
		{
			if (part.controllable)
			{
				part.triggerStop();
			}
		}
	}

	private bool moved = false;

	public void joystick(Vector2 vec)
	{
		foreach (Part part in parts)
		{
			if (part.controllable)
			{
				part.joystick(vec);
				if (!moved) {
					moved = true;;
				} else if (fired) {
					GameManager.Instance.tutorialOver = true;
				}
			}
		}
	}

	public void joystickStop()
	{
		foreach (Part part in parts)
		{
			if (part.controllable)
			{
				part.joystickStop();
			}
		}
	}

	public void deploy()
	{
		state = State.Deployed;
		parts = GetComponentsInChildren<Part>();
		foreach (Part part in parts)
		{
			part.deploy(true);
		}
	}

	public void activate()
	{
		state = State.Active;
		parts = GetComponentsInChildren<Part>();
		foreach (Part part in parts)
		{
			part.activate ();
		}
	}

	public void reset()
	{
		state = State.Inactive;
		parts = GetComponentsInChildren<Part>();
		foreach (Part part in parts)
		{
			part.reset();
		}
	}

	public void online()
	{
		state = State.Active;
		parts = GetComponentsInChildren<Part>();
		foreach (Part part in parts)
		{
			//part.online ();
		}
	}

	void Awake ()
	{
		parts = new Part[0];

	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void updateParts()
	{
		parts = GetComponentsInChildren<Part>();
		weapons = GetComponentsInChildren<Weapon> ();
		currentPowerLevel = maxPowerLevel;
		foreach (Weapon weapon in weapons) {
			currentPowerLevel -= weapon.powerUsed;
		}
	}

	public void CreatePrefabFromRobot() {
		#if UNITY_EDITOR

		PrefabUtility.CreatePrefab ("Assets/Resources/WeakRobot.prefab", this.gameObject);

		#endif
	}

	public void ActivateTutorialRobot() {
		this.transform.GetChild (0).gameObject.SetActive (true);
		activate ();
		deploy ();
		Base robotBase = GetComponentInChildren<Base> ();
		if (robotBase) {
			robotBase.on ();
		}
	}

	public void DeactivateTutorialRobot() {
		GameObject.Destroy (this.transform.GetChild (0).gameObject);
	}
}