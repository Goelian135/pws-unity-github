using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine machine, CharacterController2D controller, PlayerInput input, Rigidbody2D rb)
        : base(machine, controller, input, rb) {}

    public override void Enter()
    {
        //hier komt niks
    }

    public override void Update()
    {
        //Bij horizontale input naar run state
        if (input.Horizontal != 0)
        {
            machine.ChangeState(machine.RunState);
            return;
        }

        //Bij jump input naar jump state
        if (input.JumpPressed)
        {
            machine.ChangeState(machine.JumpState);
            return;
        }

        //Bij naar beneden naar fall state
        if (rb.velocity.y < -0.1f)
        {
            machine.ChangeState(machine.FallState);
            return;
        }

        //Bij dash input naar dash state
        if (input.DashPressed)
        {
            machine.ChangeState(machine.DashState);
            return;
        }

        //Bij attack input naar attack state
        if (input.AttackPressed)
        {
            machine.ChangeState(machine.AttackState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        //Zorg dat de speler stilstaat
        controller.Move(0, false, false);
    }

    public override void Exit()
    {
        //hier komt niks
    }
}
