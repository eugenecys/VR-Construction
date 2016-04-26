using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(ParticleSystem))]

public class Explosion : MonoBehaviour {
    
    public ParticleSystem particleSystem;
    public float explosionRadius;
    public float explosionTime;

    private int particleCount = 1500;
    private SphereCollider sp;
    private float initialRadius;

    public void explode()
    {
        particleSystem.Emit(particleCount);
        initialRadius = sp.radius;
        
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
    }

    void Awake()
    {
        sp = GetComponent<SphereCollider>();
        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
