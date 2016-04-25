using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
    ParticleSystem part;
	// Use this for initialization
	void Start () {
        part = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (part && part.isStopped)
            Destroy(gameObject);
	}
}
