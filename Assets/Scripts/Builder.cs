using UnityEngine;
using System.Collections;

public class Builder : Singleton<Builder> {

    public Component activeComponent;

    public void connectActiveComponent()
    {
        activeComponent.connect();
    }

    public void deployActiveComponent()
    {
        activeComponent.deploy();
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
	}
}
