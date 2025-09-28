using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;


    float horizontalMove = 0f;
    public float runSpeed = 40f;

    bool jump = false;
    bool dash = false;

    // Update is called once per frame
    void Update()
    {
        //get horizontal movement input
        horizontalMove = InputManager.Instance.GetKey("MoveLeft") ? -1 :
                         InputManager.Instance.GetKey("MoveRight") ? 1 : 0;

        if (InputManager.Instance.GetKeyDown("Jump"))
        {
            jump = true;
        }

        if (InputManager.Instance.GetKeyDown("Dash"))
        {
            dash = true;
        }
    }

    private void FixedUpdate()
    {
        //move the character
        controller.Move(horizontalMove *runSpeed *Time.fixedDeltaTime, jump, dash);
        jump = false;
        dash = false;
    }
}
