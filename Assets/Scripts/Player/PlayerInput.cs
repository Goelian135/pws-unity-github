using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool DashPressed { get; private set; }


    // Update is called once per frame
    void Update()
    {
        Horizontal = InputManager.Instance.GetKey("MoveLeft") ? -1f : 
                     InputManager.Instance.GetKey("MoveRight") ? 1f : 0f;
        
        JumpPressed = InputManager.Instance.GetKeyDown("Jump");
        JumpHeld = InputManager.Instance.GetKey("Jump");
        DashPressed = InputManager.Instance.GetKeyDown("Dash");
    }
}
