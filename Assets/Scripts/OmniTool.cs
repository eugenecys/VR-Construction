using UnityEngine;
using System.Collections;

public class OmniTool : MonoBehaviour {

    ViveInputManager inputManager;
    public Builder builder;

    public enum GameState
    {
        Build,
        Play
    }

    public enum Side
    {
        Left,
        Right
    }

    public GameState state;
    public Side side;

    void Awake()
    {
        inputManager = ViveInputManager.Instance;
        if (side.Equals(Side.Left))
        {
            inputManager.registerFunction(trigger, ViveInputManager.InputType.LeftTrigger);
        }
        else
        {
            inputManager.registerFunction(trigger, ViveInputManager.InputType.RightTrigger);
        }
    }

    public void trigger()
    {
        switch (state)
        {
            case GameState.Build:
                builder.trigger();
                break;
            case GameState.Play:
                break;
        }
    }

    public void touchpad()
    {
        switch (state)
        {
            case GameState.Build:
                builder.deployRobot();
                break;
            case GameState.Play:
                break;
        }
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
