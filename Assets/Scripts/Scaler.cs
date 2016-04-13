using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {
    
    public GameObject target;
    private ScaleArrow scaleObject;
    private Vector3 initialScale;
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

    public void initScale(ScaleArrow arrow)
    {
        scaleObject = arrow;
        initialScale = new Vector3(target.transform.localScale.x, target.transform.localScale.y, target.transform.localScale.z);
        initialDist = arrow.transform.localPosition.magnitude;
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
        float dScale = 0;
        foreach(ScaleArrow arrow in scaleArrows)
        {
            switch (arrow.direction)
            {
                case ScaleArrow.Direction.X:
                    dScale = tScale.x / initialScale.x;
                    arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x * dScale, 0, 0);
                    break;
                case ScaleArrow.Direction.Y:
                    dScale = tScale.y / initialScale.y;
                    arrow.transform.localPosition = new Vector3(0, arrow.transform.localPosition.y * dScale, 0);
                    break;
                case ScaleArrow.Direction.Z:
                    dScale = tScale.x / initialScale.x;
                    arrow.transform.localPosition = new Vector3(0, 0, arrow.transform.localPosition.z * dScale);
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
        initArrows();
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (scaling)
        {
            float newLength = 0;
            if (uniformScale)
            {
                newLength = scaleObject.transform.localPosition.magnitude;
                target.transform.localScale = initialScale * newLength / initialDist;
            }
            else {
                switch (scaleObject.direction)
                {
                    case ScaleArrow.Direction.X:
                        newLength = Mathf.Abs(scaleObject.transform.localPosition.x);
                        target.transform.localScale = new Vector3(initialScale.x * newLength / initialDist, initialScale.y, initialScale.z);
                        break;
                    case ScaleArrow.Direction.Y:
                        newLength = Mathf.Abs(scaleObject.transform.localPosition.y);
                        target.transform.localScale = new Vector3(initialScale.x, initialScale.y * newLength / initialDist, initialScale.z);
                        break;
                    case ScaleArrow.Direction.Z:
                        newLength = Mathf.Abs(scaleObject.transform.localPosition.z);
                        target.transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z * newLength / initialDist);
                        break;
                }
            }
        }
	}
}
