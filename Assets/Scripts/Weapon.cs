using UnityEngine;
using System.Collections;
using System;

public class Weapon : MonoBehaviour, Controllable {

    protected EventManager eventManager;
    protected ScoreManager scoreManager;

    public float coolDown;
    public float fireInteval;

    protected bool isInCD = false;
    protected bool isFiring = false;
    protected float fireCountDown;
    
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

    protected IEnumerator CoolDown(float duration) {
        isInCD = true;
        Fire();
        yield return new WaitForSeconds(duration);
        isInCD = false;
    }

    protected void Inteval() {
        if (fireCountDown >= fireInteval)
        {
            fireCountDown = 0;
            if(!isInCD) StartCoroutine(CoolDown(coolDown));
        }
        fireCountDown += Time.deltaTime;
    }

    protected virtual void Fire() {

    }

}
