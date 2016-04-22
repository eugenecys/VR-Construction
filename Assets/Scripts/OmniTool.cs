using UnityEngine;
using System.Collections;

public class OmniTool : MonoBehaviour
{
	GameManager gameManager;
	ViveInputManager inputManager;
	public Builder builder;
	public PlayMode playMode;
	public UIControls ui;
	public BlinkMechanic blink;
	public enum Side
	{
		Left,
		Right
	}

	public Side side;

	private Vector2 axis;

	void Awake ()
	{
		gameManager = GameManager.Instance;
		inputManager = ViveInputManager.Instance;

		if (side.Equals (Side.Left)) {
			inputManager.registerFunction (triggerDown, ViveInputManager.InputType.LeftTriggerDown);
			inputManager.registerFunction (touchpadDown, ViveInputManager.InputType.LeftTouchpadDown);
			inputManager.registerFunction (applicationmenuDown, ViveInputManager.InputType.LeftApplicationMenuDown);
			inputManager.registerFunction (triggerUp, ViveInputManager.InputType.LeftTriggerUp);
			inputManager.registerFunction (touchpadUp, ViveInputManager.InputType.LeftTouchpadUp);
			inputManager.registerFunction (applicationmenuUp, ViveInputManager.InputType.LeftApplicationMenuUp);
			inputManager.registerFunction (touchpadAxis, ViveInputManager.InputType.LeftTouchpadAxis);
			inputManager.registerFunction (gripDown, ViveInputManager.InputType.LeftGripDown);
		} else {
			inputManager.registerFunction (triggerDown, ViveInputManager.InputType.RightTriggerDown);
			inputManager.registerFunction (touchpadDown, ViveInputManager.InputType.RightTouchpadDown);
			inputManager.registerFunction (applicationmenuDown, ViveInputManager.InputType.RightApplicationMenuDown);
			inputManager.registerFunction (triggerUp, ViveInputManager.InputType.RightTriggerUp);
			inputManager.registerFunction (touchpadUp, ViveInputManager.InputType.RightTouchpadUp);
			inputManager.registerFunction (applicationmenuUp, ViveInputManager.InputType.RightApplicationMenuUp);
			inputManager.registerFunction (touchpadAxis, ViveInputManager.InputType.RightTouchpadAxis);
			inputManager.registerFunction (gripDown, ViveInputManager.InputType.RightGripDown);
		}
	}

	public void applicationmenuDown (params object[] args)
	{
		if (gameManager.debug) {
			builder.menu ();
			gameManager.state = GameManager.GameState.Build;
		}
	}

	public void touchpadAxis (params object[] args)
	{
		if (args.Length > 0) {
			axis = (Vector2)args [0];
		}
		touchpadDown ();
	}

	public void triggerDown (params object[] args)
	{
		switch (gameManager.state) {
		case GameManager.GameState.Build:
			builder.triggerDown ();
			break;
		case GameManager.GameState.Play:
			playMode.triggerDown ();
			break;
		case GameManager.GameState.Start:
			ui.triggerDown ();
			break;
		case GameManager.GameState.SelectBase:
			ui.triggerDown ();
			break;
		}
	}

	public void triggerUp (params object[] args)
	{
		switch (gameManager.state) {
		case GameManager.GameState.Build:
			builder.triggerUp ();
			break;
		case GameManager.GameState.Play:
			playMode.triggerUp ();
			break;
		case GameManager.GameState.Start:
			ui.triggerUp ();
			break;
		case GameManager.GameState.SelectBase:
			ui.triggerUp ();
			break;
		}
	}

	public void touchpadDown (params object[] args)
	{
		switch (gameManager.state) {
		case GameManager.GameState.Build:
			break;
		case GameManager.GameState.Play:
			playMode.touchPadDown (axis);
			break;
		case GameManager.GameState.Start:
			break;
		case GameManager.GameState.SelectBase:
			break;
		}
	}

	public void touchpadUp (params object[] args)
	{

		switch (gameManager.state) {
		case GameManager.GameState.Build:
			break;
		case GameManager.GameState.Play:
			playMode.touchPadUp ();
			break;
		case GameManager.GameState.Start:
			break;
		case GameManager.GameState.SelectBase:
			break;
		}
	}

	public void applicationmenuUp (params object[] args)
	{

	}

	public void gripDown(params object[] args) {
		switch (gameManager.state) {
		case GameManager.GameState.Build:
			break;
		case GameManager.GameState.Play:
			blink.gripDown ();
			break;
		case GameManager.GameState.Start:
			break;
		case GameManager.GameState.SelectBase:
			break;
		}
	}
    
	// Use this for initialization
	void Start ()
	{
		axis = Vector2.zero;
	}

	// Update is called once per frame
	void Update ()
	{
		KeyControls ();
	}

	private void KeyControls() {
		if (Input.GetKeyDown (KeyCode.R) && side == Side.Left) {
			triggerDown ();
		}
		if (Input.GetKeyDown (KeyCode.E)  && side == Side.Left) {
			triggerUp ();
		}
		if (Input.GetKeyDown (KeyCode.U)  && side == Side.Right) {
			triggerDown ();
		}
		if (Input.GetKeyDown (KeyCode.Y)  && side == Side.Right) {
			triggerUp ();
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			GameManager.Instance.StartGame ();
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			UIManager.Instance.SelectBaseOne ();
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			UIManager.Instance.SelectBaseTwo ();
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			Deployer.Instance.deploy ();
		}
	}
}
