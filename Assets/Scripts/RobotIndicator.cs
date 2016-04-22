using UnityEngine;
using System.Collections;

public class RobotIndicator : MonoBehaviour {
    private GameManager gameManager;
    private Robot robot;
    private GameObject m_indicator;

    public GameObject indicator;
    public GameObject headSet;
    public float rayLen;
    public float height;
    // Use this for initialization
    void Awake () {
        gameManager = GameManager.Instance;
        robot = Robot.Instance;
    }
	void Start () {
        m_indicator = Instantiate(indicator, robot.transform.position + new Vector3(0, height, 0), Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
        SetIndicatorInvisiblity();
	}

    void SetIndicatorInvisiblity() {
        if (gameManager.state == GameManager.GameState.Play)
        {
            IndicatorActivate();
        }
        else {
            m_indicator.SetActive(false);
        }
    }

    void IndicatorActivate() {
        Ray ray = new Ray(robot.transform.position, headSet.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLen))
        {
            if (hit.transform.CompareTag("building"))
            {
                m_indicator.SetActive(true);
            }
            else {
                m_indicator.SetActive(false);
            }
        }
        else {
            m_indicator.SetActive(false);
        }
    }
}
