using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    private ScoreManager scoreManager;
	public float destroyTime;

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
            col.transform.gameObject.SendMessage("GiveAttack");
            scoreManager.AddScore();
        }
    }
}
