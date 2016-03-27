using UnityEngine;
using System.Collections;

public class Builder : Singleton<Builder> {

    public Component activeComponent;
    public Vector3 spawnposition;

    public void connectActiveComponent()
    {
        activeComponent.parent.connect();
    }

    public void deployActiveComponent()
    {
        activeComponent.parent.deploy();
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
                Component component = hit.collider.gameObject.GetComponent<Component>();
                if (component == null)
                {
                    component = hit.collider.gameObject.GetComponentInParent<Component>();
                }
                if (component != null)
                {
                    activeComponent = component;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            deployActiveComponent();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            connectActiveComponent();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SpawnComponent("Cube", spawnposition);
        }
        if (Input.GetKeyDown(KeyCode.G))
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
    }
}
