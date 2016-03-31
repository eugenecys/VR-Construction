using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour {

    private Robot robot;
    private Segment activeComponent;
    private Part highlightedPart;
    private Part spawnedPart;
    public Vector3 spawnposition;
    public Transform spawnPoint;

    private ViveInputManager inputManager;

    //Delete - keyboard code
    public void connectPart()
    {
        activeComponent.parent.place();
    }

    public void placePart()
    {
        spawnedPart.place();
        if (spawnedPart.placed)
        {
            spawnedPart = null;
        }
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

    public void triggerRobot()
    {
        robot.trigger();
    }

    void Awake()
    {
        robot = Robot.Instance;
        inputManager = ViveInputManager.Instance;
    }

	// Use this for initialization
	void Start () {
	
	}

    public void trigger()
    {
        if (spawnedPart == null)
        {
            SpawnComponent();
        }
        else
        {
            placePart();
        }
    }

    public void SpawnComponent()
    {
        if (highlightedPart != null)
        {
            SpawnComponent(highlightedPart.name, spawnPoint.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Part highlight = other.GetComponent<Part>();
        if (highlight == null)
        {
            highlight = other.GetComponentInParent<Part>();
        }

        if (highlight != null)
        {
            highlightedPart = highlight;
            Debug.Log(highlightedPart);
        }
    }

    void OnTriggerExit(Collider other)
    {
        highlightedPart = null;
    }

	// Update is called once per frame
	void Update () {

        if (spawnedPart != null)
        {
            spawnedPart.resetPhysics();
            spawnedPart.transform.position = spawnPoint.position;
            spawnedPart.transform.rotation = spawnPoint.rotation;
        }
        
        //Delete - keyboard code
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            triggerRobot();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnComponent(Part.Name.Cube, spawnposition);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnComponent(Part.Name.Rod, spawnposition);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnComponent(Part.Name.Wheel, spawnposition);
        }
	}

    public void SpawnComponent(Part.Name part, Vector3 position)
    {
        string partName = "";
        switch (part)
        {
            case Part.Name.Wheel:
                partName = Constants.NAME_WHEEL;
                break;
            case Part.Name.Rod:
                partName = Constants.NAME_ROD;
                break;
            case Part.Name.Chain:
                partName = Constants.NAME_CHAIN;
                break;
            case Part.Name.Cube:
                partName = Constants.NAME_CUBE;
                break;
            case Part.Name.Gun:
                partName = Constants.NAME_GUN;
                break;
        }
        GameObject prefab = Resources.Load("Prefabs/" + partName) as GameObject;
        GameObject sObj = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
        sObj.transform.parent = this.transform;
        spawnedPart = sObj.GetComponent<Part>();
    }
}
