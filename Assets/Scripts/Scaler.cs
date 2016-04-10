using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {
    
    public GameObject target;
    public Vector3 scalePositions;
    private ScaleArrow scaleObject;
    private Vector3 scaleMultiplier;
    private bool scaling;
    public ScaleArrow X;
    public ScaleArrow Y;
    public ScaleArrow Z;
    public ScaleArrow Xn;
    public ScaleArrow Yn;
    public ScaleArrow Zn;

    public void initScale(ScaleArrow arrow)
    {
        scaleObject = arrow;
        scaling = true;
        X.hide();
        Y.hide();
        Z.hide();
        Xn.hide();
        Yn.hide();
        Zn.hide();
    }

    public void stopScale()
    {
        scaling = false;
        X.show();
        Y.show();
        Z.show();
        Xn.show();
        Yn.show();
        Zn.show();
        resetPositions();
    }

    public void resetPositions()
    {
        Vector3 tScale = target.transform.localScale;
        X.transform.localPosition = new Vector3(tScale.x / scaleMultiplier.x + transform.localPosition.x, 0, 0);
        Xn.transform.localPosition = new Vector3(-tScale.x / scaleMultiplier.x + transform.localPosition.x, 0, 0);
        Y.transform.localPosition = new Vector3(0, tScale.y / scaleMultiplier.y + transform.localPosition.y, 0);
        Yn.transform.localPosition = new Vector3(0, -tScale.y / scaleMultiplier.y + transform.localPosition.y, 0);
        Z.transform.localPosition = new Vector3(0, 0, tScale.z / scaleMultiplier.z + transform.localPosition.z);
        Zn.transform.localPosition = new Vector3(0, 0, -tScale.z / scaleMultiplier.z + transform.localPosition.z);
    }

    void Awake()
    {
        scaling = false;
        Vector3 lScale = transform.localScale;
        scaleMultiplier = new Vector3(lScale.x / scalePositions.x, lScale.y / scalePositions.y, lScale.z / scalePositions.z);
        X.scaleParent = this;
        Y.scaleParent = this;
        Z.scaleParent = this;
        Xn.scaleParent = this;
        Yn.scaleParent = this;
        Zn.scaleParent = this;

        X.direction = ScaleArrow.Direction.X;
        Xn.direction = ScaleArrow.Direction.X;
        Y.direction = ScaleArrow.Direction.Y;
        Yn.direction = ScaleArrow.Direction.Y;
        Z.direction = ScaleArrow.Direction.Z;
        Zn.direction = ScaleArrow.Direction.Z;

        X.transform.localPosition = new Vector3(scalePositions.x, 0, 0);
        Y.transform.localPosition = new Vector3(0,  scalePositions.y, 0);
        Z.transform.localPosition = new Vector3(0, 0, scalePositions.z);
        Xn.transform.localPosition = new Vector3(-scalePositions.x, 0, 0);
        Yn.transform.localPosition = new Vector3(0, -scalePositions.y, 0);
        Zn.transform.localPosition = new Vector3(0, 0, -scalePositions.z);
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
