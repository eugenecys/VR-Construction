using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager> {

    public int deployTime = 90;
	public int tutorialTime = 45;

    public Text timer;

    private float curTime;
	private float countdown;
	private bool deployed = false;
	private bool gameOver = false;
	private bool tutorial = false;
	// Use this for initialization
	void Start () {
        curTime = 0;
		countdown = deployTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (curTime >= 1)
        {
			if (countdown > 0 && deployed) {
				countdown -= 1;
				timer.text = "Time to destroy: " + countdown.ToString ();
			} else if (countdown == 0 && deployed && !gameOver) {
				Robot.Instance.destroy ();
				GameManager.Instance.EndGame ();
				gameOver = true;
				deployed = false;
				//Undeploy ();
			} else if (countdown > 0 && tutorial) {
				countdown -= 1;
				timer.text = "Time to destroy: " + countdown.ToString ();
			} else if (countdown == 0 && tutorial) {
				GameManager.Instance.GoToSelectBase ();
				tutorial = false;
			}
            curTime = 0;
        }
        curTime += Time.deltaTime;
	}

	public void StartDeployCountdown() {
		countdown = deployTime;
		deployed = true;
	}

	public void StartTutorialCountdown() {
		countdown = tutorialTime;
		tutorial = true;
	}

	private void Undeploy() {
		Robot.Instance.destroy ();
		Robot.Instance.reset ();
		Deployer.Instance.undeploy ();
		GameManager.Instance.SpawnRobotBase (GameManager.Instance.selectedBase);
		deployed = false;
	}
		
}
