using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Rigidbody))]

public class Explosion : MonoBehaviour {
    
    public ParticleSystem particleSystem;
    public float explosionRadius;
    public float explosionTime;

    private int particleCount = 3000;
	private float startupTime = 0.5f;
	private float spawnTime;
	public SphereCollider sp;
	private Rigidbody rb;
    private float initialRadius;
	bool exploding;

    public void explode()
    {
        particleSystem.Emit(particleCount);
        initialRadius = sp.radius;
		rb.velocity = Vector3.zero;
		StartCoroutine (boom());
    }

    IEnumerator boom()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / explosionTime;
            sp.radius = Mathf.Lerp(initialRadius, explosionRadius, t);
            yield return null;
        }
		sp.radius = initialRadius;
    }

	void OnTriggerEnter() {
		if (!exploding && Time.time > (spawnTime + startupTime)) {
			exploding = true;
			explode ();
		}
	}

    void Awake()
    {
		rb = GetComponent<Rigidbody> ();
        
    }

	// Use this for initialization
	void Start () {
		spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
