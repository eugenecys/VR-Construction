using UnityEngine;
using System.Collections;

public class RobotIndicator : MonoBehaviour {
    private GameManager gameManager;
    private GameObject m_indicator;
	private bool isIndicated;
	private MeshRenderer mesh;
	private Transform robotTran;

	public GameObject robot;
    public GameObject indicator;
    public GameObject headSet;
    public float rayLen;
    public float height;
    // Use this for initialization
    void Awake () {
        gameManager = GameManager.Instance;
    }
	void Start () {
		isIndicated = false;
		robotTran = null;
	}
	
	// Update is called once per frame
	void Update () {
		LocateIndicator ();
        SetIndicatorInvisiblity();
	}

	void LocateIndicator(){
		if (!isIndicated && gameManager.state == GameManager.GameState.Build) {
			robotTran = robot.transform.Find ("BaseBodyContainer");
			if (robotTran != null) {
				isIndicated = true;
				m_indicator = Instantiate(indicator, robotTran.position + new Vector3(0, height, 0), Quaternion.identity) as GameObject;
				m_indicator.transform.SetParent (robotTran);
				mesh = m_indicator.GetComponent<MeshRenderer> ();
			} 
		}
	}

    void SetIndicatorInvisiblity() {
		if (isIndicated) {
			if (gameManager.state == GameManager.GameState.Play)
			{
				IndicatorActivate();
			}
			else {
				mesh.enabled = false;
			}
		}
    }

    void IndicatorActivate() {
		Ray ray = new Ray(robotTran.transform.position, headSet.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLen))
        {
            if (hit.transform.CompareTag("building"))
            {
				mesh.enabled = true;
            }
            else {
				mesh.enabled = false;
            }
        }
        else {
			mesh.enabled = false;
        }
    }
}
