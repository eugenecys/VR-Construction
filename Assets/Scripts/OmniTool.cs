using UnityEngine;
using System.Collections;

public class OmniTool : MonoBehaviour
{
    GameManager gameManager;
    ViveInputManager inputManager;
    public Builder builder;

    //Xiqiao Add
    public PlayMode playMode;
    
    public enum Side
    {
        Left,
        Right
    }
    
    public Side side;

    void Awake()
    {
        gameManager = GameManager.Instance;
        inputManager = ViveInputManager.Instance;
        if (side.Equals(Side.Left))
        {
            inputManager.registerFunction(triggerDown, ViveInputManager.InputType.LeftTriggerDown);
            inputManager.registerFunction(touchpadDown, ViveInputManager.InputType.LeftTouchpadDown);
            inputManager.registerFunction(applicationmenuDown, ViveInputManager.InputType.LeftApplicationMenuDown);
            inputManager.registerFunction(triggerUp, ViveInputManager.InputType.LeftTriggerUp);
            inputManager.registerFunction(touchpadUp, ViveInputManager.InputType.LeftTouchpadUp);
            inputManager.registerFunction(applicationmenuUp, ViveInputManager.InputType.LeftApplicationMenuUp);
        }
        else
        {
            inputManager.registerFunction(triggerDown, ViveInputManager.InputType.RightTriggerDown);
            inputManager.registerFunction(touchpadDown, ViveInputManager.InputType.RightTouchpadDown);
            inputManager.registerFunction(applicationmenuDown, ViveInputManager.InputType.RightApplicationMenuDown);
            inputManager.registerFunction(triggerUp, ViveInputManager.InputType.RightTriggerUp);
            inputManager.registerFunction(touchpadUp, ViveInputManager.InputType.RightTouchpadUp);
            inputManager.registerFunction(applicationmenuUp, ViveInputManager.InputType.RightApplicationMenuUp);
        }
    }
    
    public void applicationmenuDown(params object[] args)
    {
        builder.menu();
    }

    public void triggerDown(params object[] args)
    {
        switch (gameManager.state)
        {
            case GameManager.GameState.Build:
                builder.triggerDown();
                break;
            case GameManager.GameState.Play:
                playMode.SetController(side, PlayMode.InputType.triggerDown);
                break;
        }
    }

    public void touchpadDown(params object[] args)
    {
        if (args.Length > 0)
        {
            // Get position of touch
        }
        switch (gameManager.state)
        {
            case GameManager.GameState.Build:
                break;
            case GameManager.GameState.Play:
                playMode.SetController(side, PlayMode.InputType.touchpadDown);
                break;
        }
    }
    
    public void touchpadUp(params object[] args) {
        if (gameManager.state == GameManager.GameState.Play) {
            playMode.SetController(side, PlayMode.InputType.touchpadUp);
        }
    }
    
    public void applicationmenuUp(params object[] args)
    {

    }

    public void triggerUp(params object[] args)
    {
        switch (gameManager.state)
        {
            case GameManager.GameState.Build:
                builder.triggerUp();
                break;
            case GameManager.GameState.Play:
                break;
        }
    }
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
