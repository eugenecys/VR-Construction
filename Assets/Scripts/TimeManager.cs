using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager> {

    public int consTime = 60;
    public int deployTime = 90;

    public Text timer;

    private float curTime;
	private float countdown;
	private bool deployed = false;

	// Use this for initialization
	void Start () {
        curTime = 0;
		countdown = consTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (curTime >= 1)
        {
			if (countdown > 0 && deployed) {
				countdown -= 1;
				timer.text = "Time to destroy: " + deployTime.ToString();
			} else {
				Undeploy ();
			}
            curTime = 0;
        }
        curTime += Time.deltaTime;
	}

	public void StartDeployCountdown() {
		countdown = deployTime;
		deployed = true;
	}

	private void Undeploy() {
		Robot.Instance.destroy ();
		Robot.Instance.reset ();
		Deployer.Instance.undeploy ();
		GameManager.Instance.SpawnRobotBase (GameManager.Instance.selectedBase);
		deployed = false;
	}
		
}
