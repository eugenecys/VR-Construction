using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(MeshRenderer))]

public class ScaleArrow : MonoBehaviour, Interactable {

    AssetManager assetManager;

    public enum Direction
    {
        X,
        Y,
        Z
    }

    public Direction direction;
    public bool negative
    {
        get; private set;
    }
    Transform dragPoint;
    bool dragging;
    public Scaler scaleParent;
    private Material highlightMaterial;
    private Material defaultMaterial;
    MeshRenderer meshRenderer;
    private Collider col;
    public Vector3 initialPosition;
    
    public void followDrag(Transform origin)
    {
        dragPoint = origin;
        scaleParent.initScale(this);
        dragging = true;
    }

    public void stopDrag()
    {
        scaleParent.stopScale();
        resetPosition();
        dragging = false;
    }

    public void setNegative(bool neg)
    {
        negative = neg;
    }

    public void resetPosition()
    {
        switch(direction)
        {
            case Direction.X:
                transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
                break;
            case Direction.Y:
                transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
                break;
            case Direction.Z:
                transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
                break;
        }
    }

    public void hide()
    {
        meshRenderer.enabled = false;
        col.enabled = false;
    }

    public void show()
    {
        meshRenderer.enabled = true;
        col.enabled = true;
    }

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        assetManager = AssetManager.Instance;
        col = GetComponent<Collider>();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (dragging)
        {
            transform.position = dragPoint.position;
        }
	}

    public void highlight()
    {
        meshRenderer.material = assetManager.arrowMaterial;
    }

    public void unhighlight()
    {
        meshRenderer.material = assetManager.blankMaterial;
    }

	void OnTriggerStay(Collider other)
	{
		
	}

	void OnTriggerExit(Collider other)
	{
		

	}
}
