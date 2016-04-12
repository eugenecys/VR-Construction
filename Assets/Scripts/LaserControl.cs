using UnityEngine;
using System.Collections;

public class LaserControl : Weapon {
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
        StopCoroutine(FireLaser());
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser() {
        mesh.enabled = true;

        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, lineLen))
        {
            if (hit.transform.tag == "building")
            {
                hit.transform.GetComponent<Building>().GiveAttack();
            }
        }

        yield return new WaitForSeconds(duration);

        mesh.enabled = false;
    }
}
