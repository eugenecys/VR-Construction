using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {
    
    public GameObject target;
    private ScaleArrow scaleObject;
    private Vector3 scaleMultiplier;
    private bool scaling;
    private ScaleArrow[] scaleArrows;
    public bool uniformScale;

    public void initScale(ScaleArrow arrow)
    {
        switch(arrow.direction)
        {
            case ScaleArrow.Direction.X:
                scaleMultiplier = new Vector3(target.transform.localScale.x / Mathf.Abs(arrow.transform.localPosition.x), 1, 1);
                break;
            case ScaleArrow.Direction.Y:
                scaleMultiplier = new Vector3(1, target.transform.localScale.y / Mathf.Abs(arrow.transform.localPosition.y), 1);
                break;
            case ScaleArrow.Direction.Z:
                scaleMultiplier = new Vector3(1, 1, target.transform.localScale.z / Mathf.Abs(arrow.transform.localPosition.z));
                break;
            case ScaleArrow.Direction.All:
                scaleMultiplier = new Vector3(target.transform.localScale.x / arrow.transform.localPosition.magnitude, target.transform.localScale.y / arrow.transform.localPosition.magnitude, target.transform.localScale.z / arrow.transform.localPosition.magnitude);
                break;
        }
        scaleObject = arrow;
        scaling = true;
        foreach (ScaleArrow child in scaleArrows)
        {
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
                    if (arrow.negative)
                    {
                        arrow.transform.localPosition = new Vector3(-tScale.x / scaleMultiplier.x, 0, 0);
                    }
                    else
                    {
                        arrow.transform.localPosition = new Vector3(tScale.x / scaleMultiplier.x, 0, 0);
                    }
                    break;
                case ScaleArrow.Direction.Y:
                    if (arrow.negative)
                    {
                        arrow.transform.localPosition = new Vector3(0, -tScale.y / scaleMultiplier.x, 0);
                    }
                    else
                    {
                        arrow.transform.localPosition = new Vector3(0, tScale.y / scaleMultiplier.x, 0);
                    }
                    break;
                case ScaleArrow.Direction.Z:
                    if (arrow.negative)
                    {
                        arrow.transform.localPosition = new Vector3(0, 0, -tScale.x / scaleMultiplier.z);
                    }
                    else
                    {
                        arrow.transform.localPosition = new Vector3(0, 0, tScale.x / scaleMultiplier.z);
                    }
                    break;
                default:
                case ScaleArrow.Direction.All:
                    break;
            }
        }
    }

    void initArrowTransforms()
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
                    if (uniformScale)
                    {
                        arrow.direction = ScaleArrow.Direction.All;
                    }
                    else
                    {
                        arrow.direction = ScaleArrow.Direction.Z;
                    }
                    arrow.transform.localPosition = new Vector3(0, 0, arrow.transform.localPosition.z);
                    arrow.setNegative(arrow.transform.localPosition.z > 0);
                }
                else
                {
                    if (uniformScale)
                    {
                        arrow.direction = ScaleArrow.Direction.All;
                    }
                    else
                    {
                        arrow.direction = ScaleArrow.Direction.X;
                    }
                    arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, 0, 0);
                    arrow.setNegative(arrow.transform.localPosition.x > 0);
                }
            }
            else
            {
                if (zMag > yMag)
                {
                    if (uniformScale)
                    {
                        arrow.direction = ScaleArrow.Direction.All;
                    }
                    else
                    {
                        arrow.direction = ScaleArrow.Direction.Z;
                    }
                    arrow.transform.localPosition = new Vector3(0, 0, arrow.transform.localPosition.z);
                    arrow.setNegative(arrow.transform.localPosition.z > 0);
                }
                else
                {
                    if (uniformScale)
                    {
                        arrow.direction = ScaleArrow.Direction.All;
                    }
                    else
                    {
                        arrow.direction = ScaleArrow.Direction.Y;
                    }
                    arrow.transform.localPosition = new Vector3(0, arrow.transform.localPosition.y, 0);
                    arrow.setNegative(arrow.transform.localPosition.y > 0);
                }
            }
            arrow.transform.rotation = Quaternion.LookRotation(arrow.transform.localPosition);

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
        initArrowTransforms();
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (scaling)
        {
            Vector3 scale = target.transform.localScale;
            float newLength = 0;
            switch (scaleObject.direction)
            {
                case ScaleArrow.Direction.X:
                    newLength = Mathf.Abs(scaleObject.transform.localPosition.x - transform.localPosition.x);
                    target.transform.localScale = new Vector3(scaleMultiplier.x * newLength, scale.y, scale.z);
                    break;
                case ScaleArrow.Direction.Y:
                    newLength = Mathf.Abs(scaleObject.transform.localPosition.y - transform.localPosition.y);
                    target.transform.localScale = new Vector3(scale.x, scaleMultiplier.y * newLength, scale.z);
                    break;
                case ScaleArrow.Direction.Z:
                    newLength = Mathf.Abs(scaleObject.transform.localPosition.z - transform.localPosition.z);
                    target.transform.localScale = new Vector3(scale.x, scale.y, scaleMultiplier.z * newLength);
                    break;
                case ScaleArrow.Direction.All:
                    newLength = (scaleObject.transform.localPosition - transform.localPosition).magnitude;
                    target.transform.localScale = new Vector3(scaleMultiplier.x * newLength, scaleMultiplier.y * newLength, scaleMultiplier.z * newLength);
                    break;
            }
        }
	}
}
