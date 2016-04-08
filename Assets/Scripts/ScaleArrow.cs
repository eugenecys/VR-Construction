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
        Z,
        All
    }

    public Direction direction;
    Vector3 dragPoint;
    bool dragging;
    public Scaler scaleParent;
    public Material highlightMaterial;
    public Material defaultMaterial;
    MeshRenderer meshRenderer;
    

    public void followDrag(Vector3 origin)
    {
        dragPoint = origin;
        scaleParent.initScale(this);
        dragging = true;
    }

    public void stopDrag()
    {
        scaleParent.stopScale();
        dragging = false;
    }

    void Awake()
    {
        scaleParent = GetComponentInParent<Scaler>();
        meshRenderer = GetComponent<MeshRenderer>();
        assetManager = AssetManager.Instance;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (dragging)
        {
            /*
            switch(direction)
            {
                case Direction.X:
                    transform.localPosition = new Vector3(dragPoint.x, 0, 0);
                    break;
                case Direction.Y:
                    transform.localPosition = new Vector3(0, dragPoint.y, 0);
                    break;
                case Direction.Z:
                    transform.localPosition = new Vector3(0, 0, dragPoint.z);
                    break;
                case Direction.All:
                    transform.position = new Vector3(dragPoint.x, dragPoint.y, dragPoint.z);
                    break;
            }*/
            transform.position = dragPoint;
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
}
