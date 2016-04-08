using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {
    
    public GameObject target;
    private ScaleArrow scaleVector;
    private Vector3 scaleMultiplier;
    private bool scaling;

    public void initScale(ScaleArrow arrow)
    {
        scaleVector = arrow;
        float scaleLength = 0;
        switch(arrow.direction)
        {
            case ScaleArrow.Direction.X:
                scaleLength = Mathf.Abs(arrow.transform.localPosition.x - transform.localPosition.x);
                scaleMultiplier = new Vector3(transform.localScale.x / scaleLength, 0, 0);
                break;
            case ScaleArrow.Direction.Y:
                scaleLength = Mathf.Abs(arrow.transform.localPosition.y - transform.localPosition.y);
                scaleMultiplier = new Vector3(0, transform.localScale.y / scaleLength, 0);
                break;
            case ScaleArrow.Direction.Z:
                scaleLength = Mathf.Abs(arrow.transform.localPosition.z - transform.localPosition.z);
                scaleMultiplier = new Vector3(0, 0, transform.localScale.z / scaleLength);
                break;
            case ScaleArrow.Direction.All:
                scaleLength = (arrow.transform.localPosition - transform.localPosition).magnitude;
                scaleMultiplier = new Vector3(transform.localScale.x / scaleLength, transform.localScale.y / scaleLength, transform.localScale.z / scaleLength);
                break;
        }
        Debug.Log("Local Length: " + scaleLength);
        scaling = true;
    }

    public void stopScale()
    {
        scaling = false;
    }

    void Awake()
    {
        scaling = false;
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (scaling)
        {
            Vector3 scale = target.transform.localScale;
            float newLength = (scaleVector.transform.localPosition - transform.localPosition).magnitude;
            switch (scaleVector.direction)
            {
                case ScaleArrow.Direction.X:
                    target.transform.localScale = new Vector3(scaleMultiplier.x * newLength, scale.y, scale.z);
                    break;
                case ScaleArrow.Direction.Y:
                    target.transform.localScale = new Vector3(scale.x, scaleMultiplier.y * newLength, scale.z);
                    break;
                case ScaleArrow.Direction.Z:
                    target.transform.localScale = new Vector3(scale.x, scale.y, scaleMultiplier.z * newLength);
                    break;
                case ScaleArrow.Direction.All:
                    target.transform.localScale = new Vector3(scaleMultiplier.x * newLength, scaleMultiplier.y * newLength, scaleMultiplier.z * newLength);
                    break;
            }
        }
	}
}
