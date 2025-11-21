using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(PlayerStateMachine machine, CharacterController2D controller, PlayerInput input, Rigidbody2D rb)
        : base(machine, controller, input, rb) { }

    public override void Enter()
    {
        //hier komt niks
    }

    public override void Update()
    {
        //naar idle
        if (input.Horizontal == 0)
        {
            machine.ChangeState(machine.IdleState);
            return;
        }

        //naar jump
        if (input.JumpPressed)
        {
            machine.ChangeState(machine.JumpState);
            return;
        }

        //naar fall
        if (rb.velocity.y < -0.1f)
        {
            machine.ChangeState(machine.FallState);
            return;
        }

        //naar dash
        if (input.DashPressed)
        {
            machine.ChangeState(machine.DashState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        float move = input.Horizontal;
        machine.controller.Move(move, false, false);
    }

    public override void Exit()
    {
        //hier komt niks
    }
}
