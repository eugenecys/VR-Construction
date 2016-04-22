using UnityEngine;
using System.Collections;

public class RobotIndicator : MonoBehaviour {
    private GameManager gameManager;
    public GameObject m_indicator;
	public bool isIndicated;
	public MeshRenderer mesh;
	public Transform robotTran;

	public GameObject robot;
    public GameObject indicator;
	public GameObject inContainer;
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
	}

	void LocateIndicator(){
		if (!isIndicated && gameManager.state == GameManager.GameState.Build) {
			robotTran = robot.GetComponentInChildren<IndicatorMark> ().gameObject.transform;
			isIndicated = true;

			m_indicator = Instantiate (indicator, inContainer.transform.position, Quaternion.identity) as GameObject;
			m_indicator.transform.SetParent (inContainer.transform);
			mesh = m_indicator.GetComponent<MeshRenderer> ();

		} else if (isIndicated && gameManager.state == GameManager.GameState.Play) {
			m_indicator.GetComponent<Animator> ().SetBool ("jump", true);
			Vector3 curPos = robotTran.position;
			inContainer.transform.position = curPos;
			IndicatorActivate ();
		} 
	}

    void IndicatorActivate() {
		Ray ray = new Ray(headSet.transform.position, robotTran.position);
        RaycastHit hit;
		Debug.DrawRay (headSet.transform.position,robotTran.position, Color.red);
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
