using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    private ScoreManager scoreManager;
	public float destroyTime;
	private GameObject dObj;
    void Awake() {
        scoreManager = ScoreManager.Instance;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Invoke ("destroyThis", destroyTime);
	}

	void destroyThis() {
		Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider col) {
		if (col.transform.tag == "building") {
			col.gameObject.SendMessage ("GiveAttack");
			scoreManager.AddScore ();
		}
	}

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "building") {
			col.gameObject.SendMessage ("GiveAttack");
			scoreManager.AddScore ();
        }
    }

}
