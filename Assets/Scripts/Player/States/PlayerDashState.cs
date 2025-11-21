using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine machine, CharacterController2D controller, PlayerInput input, Rigidbody2D rb)
        : base(machine, controller, input, rb) { }


    public override void Enter()
    {
        controller.Move(0, false, true); //trigger dash via controller
    }

    public override void Update()
    {
        if (!controller.isDashing)
        {
            //naar idle
            if (controller.isGrounded)
            {
                machine.ChangeState(machine.IdleState);
                return;
            }

            //naar fall
            if (rb.velocity.y < -0.1f)
            {
                machine.ChangeState(machine.FallState);
                return;
            }
        }

        else
        {
            controller.Move(0, false, true); //zorg dat dash afgemaakt wordt
        }
    }

    public override void FixedUpdate()
    {
        //geen movement tijdens dash
    }

    public override void Exit()
    {
        //hier komt niks
    }
}
