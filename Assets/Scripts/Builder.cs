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
	private AudioSource audioSource;
	private SoundManager soundManager; 

    private float refreshDelay = 0.2f;
    public bool triggered = false;

	private LaserPointer laser;

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
		if (!laser) {
			Long.enabled = false;
			Short.enabled = false;
		}
    }

    void enableCollider()
	{	
		if (!laser) {
			Long.enabled = true;
			Short.enabled = true;
		}
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
            enableCollider();
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

    public void triggerRobotStop()
    {
        robot.triggerStop();
    }

    void Awake()
    {
        robot = Robot.Instance;
        inputManager = ViveInputManager.Instance;
        deployer = Deployer.Instance;
		laser = this.GetComponent<LaserPointer> ();
		audioSource = this.GetComponent<AudioSource> ();
		soundManager = SoundManager.Instance;
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
		

        if (!triggered)
        {
            return;
        }
        triggered = false;
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
				audioSource.PlayOneShot(soundManager.trashSound);
            }
            else
            {
                placeParts();
            }
        }
		contactObject = null;
		if (laser) {
			laser.active = true;
		}
    }

    public void triggerDown()
    {
		
        if (triggered)
        {
            return;
        }
        triggered = true;
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
                        contactObject = null;
                    }
                }
                else
                {
                    if (part.template)
                    {
                        SpawnComponent(part);
                        contactObject = null;
                    }
                    else
                    {
                        MoveComponent(part);
                        contactObject = null;
                    }
                }
            }
            else
            {
                scaleArrow.followDrag(transform);
				if (!UIManager.scaledForFirstTime) {
					UIManager.Instance.ShowScaleControls (false);
					UIManager.scaledForFirstTime = true;
				}
            }
			if (laser) {
				laser.active = false;
			}
        }
    }

    public void DestroyRobot()
    {
        robot.destroy();
    }


    public void MoveComponent(Part part)
    {
        disableCollider();
        part.disconnect();
        part.transform.parent = this.transform;
        part.unplace();
		audioSource.PlayOneShot(soundManager.pickupSound);
    }

    public void SpawnComponent(Part part)
    {	
		spawnposition = spawnPoint.position;
		SpawnComponent(part, spawnposition);
        disableCollider();
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
			if (contactObject != null) {
				Interactable iObj = contactObject.GetComponent<Interactable> ();
				if (iObj != null) {
					iObj.unhighlight ();
				}
			}
			contactObject = null;
    }
    
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnComponent(Part part, Vector3 position)
    {
        GameObject prefab = Resources.Load("Prefabs/" + part.name) as GameObject;
        GameObject sObj = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
		sObj.transform.parent = this.transform;
        Part spawnedPart = sObj.GetComponent<Part>();
        spawnedPart.template = false;
        spawnedPart.evaluateState();
		audioSource.PlayOneShot(soundManager.pickupSound);

		if (!UIManager.pickedUpForFirstTime) {
			UIManager.Instance.ShowPickUpControls (false);
			UIManager.Instance.ShowScaleControls (true);
			UIManager.pickedUpForFirstTime = true;
		}
    }
		

	public void SetContactObject(GameObject contact) {
		contactObject = contact;
	}
}
