using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine machine, CharacterController2D controller, PlayerInput input, Rigidbody2D rb)
        : base(machine, controller, input, rb) { }

    private bool jumpStarted = false;

    public override void Enter()
    {
        jumpStarted = false;

        //trigger sprong via controller
        controller.Move(0, true, false, false);
        jumpStarted = true;
    }

    public override void Update()
    { 
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
        machine.controller.Move(move, false, false, false);
    }

    public override void Exit()
    {
        //hier komt niks
    }
}
