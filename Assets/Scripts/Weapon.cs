using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour, Controllable {

    protected EventManager eventManager;
    protected ScoreManager scoreManager;

    public float coolDown;
	public int powerUsed = 0;

    protected bool coolingDown = false;
    protected bool triggerDown = false;
	protected bool canfire = false;
    protected float fireCountDown;


    protected float multi = 1;

    public float Multi {
        get { return multi; }
        set { multi = value; }
    }
    
    public virtual void joystick(Vector2 coordinates)
    {
        
    }

    public virtual void joystickStop()
    {

    }

    public virtual void trigger()
    {
		
    }

    public virtual void triggerStop()
    {
		
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void AddScore() {
        if (scoreManager) {
            scoreManager.AddScore();
        }
    }

	protected void cooldownWeapon() {
		fireCountDown = Time.time;
		coolingDown = true;
	}

    protected virtual void Fire() {

    }

    protected virtual void AmmoScale(float scale) {

    }

}
