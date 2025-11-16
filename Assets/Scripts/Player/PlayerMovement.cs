using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    float horizontalMove = 0f;
    public float runSpeed = 40f;

    bool jumpPressed = false;
    bool jumpHeld = false;
    bool dashPressed = false;

    // Update is called once per frame
    void Update()
    {
        //get horizontal movement input
        horizontalMove = InputManager.Instance.GetKey("MoveLeft") ? -1 :
                         InputManager.Instance.GetKey("MoveRight") ? 1 : 0;

        if (InputManager.Instance.GetKeyDown("Jump"))
        {
            jumpPressed = true;
        }

        jumpHeld = InputManager.Instance.GetKey("Jump");

        if (InputManager.Instance.GetKeyDown("Dash"))
        {
            dashPressed = true;
        }
    }

    private void FixedUpdate()
    {
        MoveInput input = new MoveInput
        {
            horizontal = horizontalMove,
            jumpPressed = jumpPressed,
            jumpHeld = jumpHeld,
            dashPressed = dashPressed
        };

        controller.ApplyInput(input);

        // Reset edge-press inputs
        jumpPressed = false;
        dashPressed = false;
    }
}
