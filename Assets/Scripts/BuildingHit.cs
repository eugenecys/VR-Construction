using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingHit : MonoBehaviour {
    public Text score;
    private ScoreManager s;
    //public GameObject DestroyedObject;

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.impactForceSum.magnitude > 25f)
    //    {
    //        DestroyIt();
    //    }
    //}

    //void OnTriggerEnter(Collider col) {
    //    if (col.tag == "weapon") {
    //        DestroyIt();
    //    }
    //}

    //void DestroyIt()
    //{
    //    if (DestroyedObject)
    //    {
    //        Instantiate(DestroyedObject, transform.position, transform.rotation);
    //    }
    //    Destroy(gameObject);
    //}

    void Start() {
        s = ScoreManager.Instance;
    }

    //void OnCollisionEnter(Collision c) {
    //    if (c.gameObject.CompareTag("building"))
    //    {
    //        c.gameObject.SendMessage("GiveAttack");
    //        int score_count = s.GetScore() + 1;
    //        score.text = "Score : " + score_count.ToString();
    //        s.SetScore(score_count);
    //        //EffectManager.Instance.CameraEffects(0.1f);
    //        //if ((state == RobotState.dodging) || (state == RobotState.damaged))
    //        //{
    //        //    rb.velocity = oldVelo;
    //        //}
    //    }
    //}
}
