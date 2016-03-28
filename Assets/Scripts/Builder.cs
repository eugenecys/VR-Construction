using UnityEngine;
using System.Collections;

public class Builder : Singleton<Builder> {

    public Robot robot;
    public Segment activeComponent;
    public Vector3 spawnposition;

    public void connectPart()
    {
        activeComponent.parent.place();
    }

    public void deactivateRobot()
    {
        robot.reset();
    }

    public void activateRobot()
    {
        robot.activate();
    }

    public void deployRobot()
    {
        robot.deploy();
    }

    void Awake()
    {
        robot = Robot.Instance;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Segment component = hit.collider.gameObject.GetComponent<Segment>();
                if (component == null)
                {
                    component = hit.collider.gameObject.GetComponentInParent<Segment>();
                }
                if (component != null)
                {
                    activeComponent = component;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            deactivateRobot();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            activateRobot();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            deployRobot();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            connectPart();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnComponent("Cube", spawnposition);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnComponent("Cylinder", spawnposition);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnComponent("Wheel", spawnposition);
        }
	}

    public void SpawnComponent(string name, Vector3 position)
    {
        GameObject prefab = Resources.Load("Prefabs/" + name) as GameObject;
        GameObject sObj = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
        sObj.transform.parent = robot.transform;
    }
}
