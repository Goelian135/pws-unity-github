using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerStateMachine machine, CharacterController2D controller, PlayerInput input, Rigidbody2D rb)
        : base(machine, controller, input, rb) { }

    public override void Enter()
    {
        //hier komt niks
    }

    public override void Update()
    {
        //naar idle
        if (controller.isGrounded)
        {
            machine.ChangeState(machine.IdleState);
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
