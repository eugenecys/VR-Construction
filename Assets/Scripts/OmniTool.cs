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
            inputManager.registerFunction(trigger, ViveInputManager.InputType.LeftTrigger);
            inputManager.registerFunction(touchpad, ViveInputManager.InputType.LeftTouchpad);
            inputManager.registerFunction(applicationmenu, ViveInputManager.InputType.LeftApplicationMenu);
        }
        else
        {
            inputManager.registerFunction(trigger, ViveInputManager.InputType.RightTrigger);
            inputManager.registerFunction(touchpad, ViveInputManager.InputType.RightTouchpad);
            inputManager.registerFunction(applicationmenu, ViveInputManager.InputType.RightApplicationMenu);
        }
    }

    public void applicationmenu()
    {
        gameManager.build();
        builder.menu();
    }

    public void trigger()
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

    public void touchpad()
    {
        switch (gameManager.state)
        {
            case GameManager.GameState.Build:
                builder.deployRobot();
                gameManager.play();
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
