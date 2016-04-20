using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    private ScoreManager scoreManager;

    void Awake() {
        scoreManager = ScoreManager.Instance;
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider col) {
        if (col.transform.tag == "building") {
            col.transform.gameObject.SendMessage("GiveAttack");
            scoreManager.AddScore();
        }
    }
}
