﻿using UnityEngine;
using System.Collections;

public class Drill : Weapon {
    public Animator anim;
    public float damageDis;
    private bool m_rotate;
    void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rotate) {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.up);
            if (Physics.Raycast(ray, out hit, damageDis))
            {
                if (hit.transform.tag == "building")
                {
                    hit.transform.gameObject.SendMessage("GiveAttack");
                }
            }
        }
    }

    public override void trigger()
    {
        anim.SetBool("rotate", true);
        m_rotate = true;
        StopCoroutine(DrillRotate());
        StartCoroutine(DrillRotate());
        //throw new NotImplementedException();
    }

    IEnumerator DrillRotate() {
        
        yield return new WaitForSeconds(3f);
        anim.SetBool("rotate", false);
        m_rotate = false;
    }
}
