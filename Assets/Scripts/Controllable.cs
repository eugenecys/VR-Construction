using UnityEngine;

public interface Controllable {

    void trigger();
    void triggerStop();
    void joystick(Vector2 coordinates);
    void joystickStop();
}
