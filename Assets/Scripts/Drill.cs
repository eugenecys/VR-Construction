using UnityEngine;
using System.Collections;

public class Drill : Weapon {
    public Animator anim;
    public float damageDis;
    private bool m_rotate;
    void Awake()
    {
        scoreManager = ScoreManager.Instance;
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
                    scoreManager.AddScore();
                }
            }
        }
    }

    public override void trigger()
    {

        //StopCoroutine(DrillRotate());
        //StartCoroutine(DrillRotate());
        //throw new NotImplementedException();
        RotateDrill();
    }

    //IEnumerator DrillRotate() {
        
    //    yield return new WaitForSeconds(3f);
    //    anim.SetBool("rotate", false);
    //    m_rotate = false;
    //}

    public override void triggerStop()
    {
        StopDrill();
    }

    void RotateDrill() {
        anim.SetBool("rotate", true);
        m_rotate = true;
    }

    void StopDrill() {
        anim.SetBool("rotate", false);
        //StopCoroutine(DrillRotate());
        m_rotate = false;
    }
}
