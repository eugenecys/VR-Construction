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

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "building") {
			dObj = col.gameObject;
			Invoke ("DestroyBuilding", 0.1f);
        }
    }

	void DestroyBuilding () {
		if (dObj != null) {
			dObj.SendMessage ("GiveAttack");
			scoreManager.AddScore ();
		}
	}
}
