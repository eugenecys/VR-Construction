using UnityEngine;

// This class is used to move UI elements in ways that are
// generally useful when using VR, specifically looking at
// the camera and rotating so they're always in front of
// the camera.
public class UIMovementVR : MonoBehaviour
{
	public bool m_LookatCamera = true;
	// Whether the UI element should rotate to face the camera.
	public Transform m_UIElement;
	// The transform of the UI to be affected.
	public Transform m_Following;
	// The transform of the object we are following
	public bool m_RotateWithObj;
	// Whether the UI should rotate with the camera so it is always in front.
	public float m_FollowSpeed = 10f;
	// The speed with which the UI should follow the camera.
	public bool followY = false;
	// The distance the UI should stay from the camera when rotating with it.
	public float m_DistanceFromCamera;


	private void Start ()
	{
	
	}


	private void Update ()
	{
		// If the UI should look at the camera set it's rotation to point from the UI to the camera.
		if (m_LookatCamera)
			m_UIElement.rotation = Quaternion.LookRotation (m_UIElement.position - m_Following.position);

		if (m_RotateWithObj) {
			Vector3 targetDirection = m_Following.forward.normalized; 
			Vector3 targetPosition = m_Following.position + targetDirection * m_DistanceFromCamera;

			if (!followY) {
				targetPosition.y = m_UIElement.position.y;
			} 
			targetPosition = Vector3.Lerp (m_UIElement.position, targetPosition, m_FollowSpeed * Time.deltaTime);
			m_UIElement.position = targetPosition;
		}
	}
}