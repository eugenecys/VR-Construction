using UnityEngine;
using System.Collections;
using System;

public class LaserControl : Weapon,Controllable {
    public float lineLen;
    public float duration;
    public GameObject laser;
    private MeshRenderer mesh;
    // Use this for initialization
    void Start () {
        mesh = laser.GetComponent<MeshRenderer>();
        mesh.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public override void fire()
    {
        
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

    public void trigger()
    {
        StopCoroutine(FireLaser());
        StartCoroutine(FireLaser());
        //throw new NotImplementedException();
    }

    public void joystick(Vector2 coordinates)
    {
        throw new NotImplementedException();
    }
}
