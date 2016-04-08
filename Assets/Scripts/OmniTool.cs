using UnityEngine;
using System.Collections;

public class OmniTool : MonoBehaviour
{
    GameManager gameManager;
    ViveInputManager inputManager;
    public Builder builder;
    
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
        }
        else
        {
            inputManager.registerFunction(triggerDown, ViveInputManager.InputType.RightTriggerDown);
            inputManager.registerFunction(touchpadDown, ViveInputManager.InputType.RightTouchpadDown);
            inputManager.registerFunction(applicationmenuDown, ViveInputManager.InputType.RightApplicationMenuDown);
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
                builder.trigger();
                break;
            case GameManager.GameState.Play:
                break;
        }
    }

    public void touchpadDown(params object[] args)
    {
        switch (gameManager.state)
        {
            case GameManager.GameState.Build:
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
