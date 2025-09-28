using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;


    float horizontalMove = 0f;
    public float runSpeed = 40f;

    bool jump = false;

    public float dashCooldown = 0.3f;
    public bool isDashing = false;
    [SerializeField] private bool canDash = true;

    // Update is called once per frame
    void Update()
    {
        //check if we can dash again
        if (!canDash && !isDashing && Time.time > controller.dashTimeStart + controller.dashDuration + dashCooldown)
        {
            canDash = true;
        }

        //get horizontal movement input
        horizontalMove = InputManager.Instance.GetKey("MoveLeft") ? -1 :
                         InputManager.Instance.GetKey("MoveRight") ? 1 : 0;

        //check for jump input
        if (InputManager.Instance.GetKeyDown("Jump"))
        {
            jump = true;
        }

        //check for dash input
        if (InputManager.Instance.GetKeyDown("Dash") && canDash && !isDashing)
        {
            isDashing = true;
            canDash = false;
        }
    }

    private void FixedUpdate()
    {
        //move the character
        controller.Move(horizontalMove *runSpeed *Time.fixedDeltaTime, jump);
        jump = false;
    }
}
