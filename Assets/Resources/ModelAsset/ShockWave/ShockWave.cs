using UnityEngine;
using System.Collections;

public class ShockWave : MonoBehaviour {
    Rigidbody rg;
    SphereCollider collider;
    float maxRadius = 50f;
    float duration = 10f;
    void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.radius = 0;
        rg = GetComponent<Rigidbody>();
        StartCoroutine(Expand());
    }

    IEnumerator Expand()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            collider.radius = maxRadius * t;
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "building")
            col.gameObject.SendMessage("GiveAttack");
    }
}
