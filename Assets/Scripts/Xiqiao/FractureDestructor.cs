using UnityEngine;
using System.Collections;

public class FractureDestructor : MonoBehaviour {
    public float bornTime;
	// Use this for initialization
	void Start () {
        Invoke("DestroyFracture", bornTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DestroyFracture() {
        Destroy(gameObject);
    }
}
