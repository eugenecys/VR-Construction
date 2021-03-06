﻿using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {
    
    public GameObject target;
    public float maxX;
    public float maxY;
    public float maxZ;
    public float minX;
    public float minY;
    public float minZ;

    private ScaleArrow scaleObject;
    private Vector3 initialScale;
    private float distMultiplier;

	[SerializeField]
    private float initialDist;

    private bool scaling;
    private ScaleArrow[] scaleArrows;
    public bool uniformScale;
    private ScaleArrow X;
    private ScaleArrow Xn;
    private ScaleArrow Y;
    private ScaleArrow Yn;
    private ScaleArrow Z;
    private ScaleArrow Zn;

    private Weapon wp;

    public void initScale(ScaleArrow arrow)
    {
        scaleObject = arrow;
        initialScale = new Vector3(target.transform.localScale.x, target.transform.localScale.y, target.transform.localScale.z);
        initialDist = arrow.transform.localPosition.magnitude;
        scaling = true;
        switch(arrow.direction)
        {
            case ScaleArrow.Direction.X:
                distMultiplier = Mathf.Abs(arrow.transform.localPosition.x / target.transform.localScale.x);
                break;
            case ScaleArrow.Direction.Y:
                distMultiplier = Mathf.Abs(arrow.transform.localPosition.y / target.transform.localScale.y);
                break;
            case ScaleArrow.Direction.Z:
                distMultiplier = Mathf.Abs(arrow.transform.localPosition.z / target.transform.localScale.z);
                break;
        }
        foreach (ScaleArrow child in scaleArrows)
        {
            child.initialPosition = child.transform.localPosition;
            child.hide();
        }
    }

    public void stopScale()
    {
        scaling = false;
        foreach(ScaleArrow arrow in scaleArrows)
        {
            arrow.show();
        }
        resetPositions();
    }

    public void resetPositions()
    {
        Vector3 tScale = target.transform.localScale;
        foreach(ScaleArrow arrow in scaleArrows)
        {
            switch (arrow.direction)
            {
                case ScaleArrow.Direction.X:
                    arrow.transform.localPosition = new Vector3(tScale.x * distMultiplier * (arrow.negative ? -1 : 1), 0, 0);
                    break;
                case ScaleArrow.Direction.Y:
                    arrow.transform.localPosition = new Vector3(0, tScale.y * distMultiplier * (arrow.negative ? -1 : 1), 0);
                    break;
                case ScaleArrow.Direction.Z:
                    arrow.transform.localPosition = new Vector3(0, 0, tScale.z * distMultiplier * (arrow.negative ? -1 : 1));
                    break;
            }
        }
    }

    void initArrows()
    {
        foreach (ScaleArrow arrow in scaleArrows)
        {
            float xMag = Mathf.Abs(Vector3.Dot(arrow.transform.localPosition, Vector3.right));
            float yMag = Mathf.Abs(Vector3.Dot(arrow.transform.localPosition, Vector3.up));
            float zMag = Mathf.Abs(Vector3.Dot(arrow.transform.localPosition, Vector3.forward));
            if (xMag > yMag)
            {
                if (zMag > xMag)
                {
                    arrow.direction = ScaleArrow.Direction.Z;
                    arrow.transform.localPosition = new Vector3(0, 0, arrow.transform.localPosition.z);
                    if (arrow.transform.localPosition.z < 0)
                    {
                        arrow.setNegative(true);
                        Zn = arrow;
                    } else
                    {
                        arrow.setNegative(false);
                        Z = arrow;
                    }
                }
                else
                {
                    arrow.direction = ScaleArrow.Direction.X;
                    arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, 0, 0);
                    if (arrow.transform.localPosition.x < 0)
                    {
                        arrow.setNegative(true);
                        Xn = arrow;
                    }
                    else
                    {
                        arrow.setNegative(false);
                        X = arrow;
                    }
                }
            }
            else
            {
                if (zMag > yMag)
                {
                    arrow.direction = ScaleArrow.Direction.Z;
                    arrow.transform.localPosition = new Vector3(0, 0, arrow.transform.localPosition.z);
                    if (arrow.transform.localPosition.z < 0)
                    {
                        arrow.setNegative(true);
                        Zn = arrow;
                    }
                    else
                    {
                        arrow.setNegative(false);
                        Z = arrow;
                    }
                }
                else
                {
                    arrow.direction = ScaleArrow.Direction.Y;
                    arrow.transform.localPosition = new Vector3(0, arrow.transform.localPosition.y, 0);
                    if (arrow.transform.localPosition.y < 0)
                    {
                        arrow.setNegative(true);
                        Yn = arrow;
                    }
                    else
                    {
                        arrow.setNegative(false);
                        Y = arrow;
                    }
                }
            }
            arrow.transform.localRotation = Quaternion.LookRotation(arrow.transform.localPosition);
        }
    }

    void Awake()
    {
        scaling = false;
        Vector3 lScale = target.transform.localScale;
        scaleArrows = GetComponentsInChildren<ScaleArrow>();
        foreach(ScaleArrow arrow in scaleArrows)
        {
            arrow.scaleParent = this;
        }
        initArrows();
    }
    
	// Use this for initialization
	void Start () {
        wp = target.GetComponentInChildren<Weapon>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (scaling)
        {
            float newLength = 0;
            float multi = 0;
            
            if (uniformScale)
            {
                newLength = scaleObject.transform.localPosition.magnitude;
                multi = newLength / initialDist;
                if (initialScale.x * multi > maxX || initialScale.y * multi > maxY || initialScale.z * multi > maxZ)
                {
                    SetScale(maxX / initialScale.x);
                    //initialScale = new Vector3(maxX, maxY, maxZ);
                    target.transform.localScale = new Vector3(maxX, maxY, maxZ);
                    return;
                }
                else if (initialScale.x * multi < minX || initialScale.y * multi < minY || initialScale.z * multi < minZ) {
                    SetScale(minX / initialScale.x);
                    //initialScale = new Vector3(maxX, maxY, maxZ);
                    target.transform.localScale = new Vector3(minX, minY, minZ);
                    return;
                }
                target.transform.localScale = initialScale * newLength / initialDist;
            }
            else {
                switch (scaleObject.direction)
                {
                    case ScaleArrow.Direction.X:
                        newLength = Mathf.Abs(scaleObject.transform.localPosition.x);
                        multi = newLength / initialDist;
                        if (initialScale.x * multi > maxX)
                        {
                            SetScale(maxX / initialScale.x);
                            target.transform.localScale = new Vector3(maxX, initialScale.y, initialScale.z);
                            return;
                        }
                        else if (initialScale.x * multi < minX)
                        {
                            SetScale(minX / initialScale.x);
                            //initialScale = new Vector3(maxX, maxY, maxZ);
                            target.transform.localScale = new Vector3(minX, initialScale.y, initialScale.z);
                            return;
                        }
                        target.transform.localScale = new Vector3(initialScale.x * newLength / initialDist, initialScale.y, initialScale.z);
                        break;
                    case ScaleArrow.Direction.Y:
                        newLength = Mathf.Abs(scaleObject.transform.localPosition.y);
                        multi = newLength / initialDist;
                        if (initialScale.y * multi > maxY)
                        {
                            SetScale(maxY / initialScale.y);
                            target.transform.localScale = new Vector3(initialScale.x, maxY, initialScale.z);
                            return;
                        }
                        else if (initialScale.y * multi < minY)
                        {
                            SetScale(minY / initialScale.y);
                            //initialScale = new Vector3(maxX, maxY, maxZ);
                            target.transform.localScale = new Vector3(initialScale.x, minY, initialScale.z);
                            return;
                        }
                        target.transform.localScale = new Vector3(initialScale.x, initialScale.y * newLength / initialDist, initialScale.z);
                        break;
                    case ScaleArrow.Direction.Z:
                        newLength = Mathf.Abs(scaleObject.transform.localPosition.z);
                        multi = newLength / initialDist;
                        if (initialScale.z * multi > maxZ)
                        {
                            SetScale(maxZ / initialScale.z);
                            target.transform.localScale = new Vector3(initialScale.x, initialScale.y, maxZ);
                            return;
                        }
                        else if (initialScale.z * multi < minZ)
                        {
                            SetScale(minZ / initialScale.z);
                            //initialScale = new Vector3(maxX, maxY, maxZ);
                            target.transform.localScale = new Vector3(initialScale.x, initialScale.y, minZ);
                            return;
                        }
                        target.transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z * newLength / initialDist);
                        break;
                }
            }
                SetScale(multi);
        }
	}

    void SetScale(float multi) {
        if (wp)
        {
            wp.Multi = multi;
        }
    }
}
