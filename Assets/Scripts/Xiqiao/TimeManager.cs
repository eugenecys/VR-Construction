using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    private static TimeManager singleton;
    public static TimeManager GetSingleton() {
        if(singleton == null) singleton = new TimeManager();
        return singleton;
    }

    public int consTime = 60;
    public int deployTime = 90;

    public Text cons;

    private float curTime;
    
	// Use this for initialization
	void Start () {
        curTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (curTime >= 1)
        {
            if (consTime > 0) consTime -= 1;
            else deployTime -= 1;
            curTime = 0;
        }
        curTime += Time.deltaTime;
        if (consTime > 0)
        {
            cons.text = "Time to construct: " + consTime.ToString();
        }
        else {
            cons.text = "Time to destroy: " + deployTime.ToString();
        }
        
	}
}
