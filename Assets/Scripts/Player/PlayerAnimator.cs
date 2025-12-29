using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header ("References")]
    public CharacterController2D controller;
    public Animator animator;

    private void Start()
    {
        if (controller == null) { controller = GetComponentInParent<CharacterController2D>(); }
        if (animator == null) { animator = GetComponent<Animator>(); }
    }

    private void OnEnable()
    {
        controller.onJump += Jump;
        controller.onDash += Dash;
    }

    private void OnDisable()
    {
        controller.onJump -= Jump;
        controller.onDash -= Dash;
    }

    // Update is called once per frame
    void Update()
    {
        //touching ground
        if (controller.isGrounded)
        {
            animator.SetBool("Grounded", true);
        }
        else { animator.SetBool("Grounded", false); }

        //checks if moving or not
        animator.SetFloat("Speed", Mathf.Abs(controller.rb.velocity.x));

        //checks if player is dashing
        if (controller.isDashing)
        {
            animator.SetBool("isDashing", true);
        }
        else { animator.SetBool("isDashing", false); }

        //checks if player is going up or down
        if (controller.rb.velocity.y > controller.apexThresehold)
        {
            animator.SetBool("Upward", true);
        }
        else
        {
            animator.SetBool("Upward", false);
        }
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
    }

    private void Dash()
    {
        animator.SetTrigger("Dash");
    }
}
