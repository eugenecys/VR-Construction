using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Builder : MonoBehaviour {

    private Robot robot;
    private Segment activeComponent;
    private GameObject contactObject;
    public Vector3 spawnposition;
    public Transform spawnPoint;
    public Part[] childParts;
    Deployer deployer;
    public Builder other;

    public Collider Long;
    public Collider Short;

    private ViveInputManager inputManager;

    public enum ColliderState
    {
        Far,
        Close
    }

    public ColliderState colliderState;

    public void setColliderState(ColliderState state)
    {
        colliderState = state;
        switch(state)
        {
            case ColliderState.Close:
                
                break;
            case ColliderState.Far:
                break;
        }
    }

    void disableCollider()
    {
        Long.enabled = false;
        Short.enabled = false;
    }

    void enableCollider()
    {
        Long.enabled = true;
        Short.enabled = true;
    }

    //Delete - keyboard code
    public void connectPart()
    {
        activeComponent.parent.place();
    }

    public void placeParts()
    {
        foreach (Part part in childParts)
        {
            part.place();
            if (part.placed)
            {
                part.transform.parent = robot.transform;
                part.resetPhysics();
            }
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
        deployer = Deployer.Instance;
    }

	// Use this for initialization
	void Start () {
	
	}

    public void menu()
    {
        DestroyRobot();
        deployer.undeploy();
        robot.reset();
    }

    public void triggerUp()
    {
        enableCollider();
        childParts = GetComponentsInChildren<Part>();
        if (childParts == null || childParts.Length == 0)
        {
            if (contactObject != null)
            {
                ScaleArrow scaleArrow = contactObject.GetComponent<ScaleArrow>();
                if (scaleArrow == null)
                {

                }
                else
                {
                    scaleArrow.stopDrag();
                }
            }
        }
        else
        {
            List<Part> markedParts = new List<Part>();
            foreach (Part part in childParts)
            {
                if (part.markedForDelete)
                {
                    markedParts.Add(part);
                }
            }
            if (markedParts.Count > 0)
            {
                foreach (Part markedPart in markedParts)
                {
                    Destroy(markedPart.gameObject);
                }
            }
            else
            {
                placeParts();
            }
        }
    }

    public void triggerDown()
    {
        disableCollider();
        if (contactObject != null)
        {
            ScaleArrow scaleArrow = contactObject.GetComponent<ScaleArrow>();
            if (scaleArrow == null)
            {
                Part part = contactObject.GetComponentInParent<Part>();
                if (part == null)
                {
                    Deployer deployer = contactObject.GetComponent<Deployer>();
                    if (deployer == null)
                    {

                    }
                    else
                    {
                        deployRobot();
                        deployer.deploy();
                    }
                }
                else
                {
                    if (part.template)
                    {
                        SpawnComponent(part);
                    }
                    else
                    {
                        MoveComponent(part);
                    }
                }
            }
            else
            {
                scaleArrow.followDrag(transform);
            }
        }
    }

    public void DestroyRobot()
    {
        robot.destroy();
    }

    public void MoveComponent(Part part)
    {
        List<Part> parts = part.getConnectedParts();
        foreach(Part child in parts)
        {
            child.transform.parent = this.transform;
            child.unplace();
        }
    }

    public void SpawnComponent(Part part)
    {
        SpawnComponent(part.name, spawnPoint.position);
    }

    void OnTriggerStay(Collider other)
    {
        contactObject = other.gameObject;
        Interactable iObj = contactObject.GetComponent<Interactable>();
        if (iObj != null)
        {
            iObj.highlight();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (contactObject != null)
        {
            Interactable iObj = contactObject.GetComponent<Interactable>();
            if (iObj != null)
            {
                iObj.unhighlight();
            }
            contactObject = null;
        }
    }
    
	// Update is called once per frame
	void Update () {
                
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
            case Part.Name.WheelR:
                partName = Constants.NAME_WHEEL_REVERSE;
                break;
            case Part.Name.Propeller:
                partName = Constants.NAME_PROPELLER;
                break;
            case Part.Name.Saw:
                partName = Constants.NAME_SAW;
                break;
        }
        GameObject prefab = Resources.Load("Prefabs/" + partName) as GameObject;
        GameObject sObj = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
        sObj.transform.parent = this.transform;
        Part spawnedPart = sObj.GetComponent<Part>();
        spawnedPart.template = false;
        spawnedPart.evaluateState();
    }
}
