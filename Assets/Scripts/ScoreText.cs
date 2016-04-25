using UnityEngine;
using System.Collections;

public class ScoreText : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SelfDestroy(float time) {
        Destroy(gameObject, time);
    }
}
