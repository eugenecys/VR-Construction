using UnityEngine;
using System.Collections;

public class WaveGenerator : Weapon {

    public GameObject wave;
    public float waveVelocity;
    public Transform firePosition;

    void Awake()
    {
        eventManager = EventManager.Instance;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void trigger()
    {
        GameObject sObj = Instantiate(wave, firePosition.position, Quaternion.identity) as GameObject;
        Rigidbody rb = sObj.GetComponent<Rigidbody>();
        rb.velocity = waveVelocity * transform.up;
        eventManager.addEvent(() => Destroy(sObj), 1f, true);
        //throw new NotImplementedException();
    }
}
