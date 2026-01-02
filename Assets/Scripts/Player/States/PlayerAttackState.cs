using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(PlayerStateMachine machine, CharacterController2D controller, PlayerInput input, Rigidbody2D rb)
        : base(machine, controller, input, rb) 
    {    }

    float attackDuration = 0.4f;
    private float timer;

    public override void Enter()
    {
        timer = attackDuration;
        controller.Move(0, false, false, true); //trigger attack via controller
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            machine.ChangeState(machine.IdleState);
        }

        else 
        { 
        //Zorg dat de speler stilstaat
        controller.Move(0, false, false, false);
        }
    }

    public override void FixedUpdate()
    {
        //geen movement tijdens attack
    }

    public override void Exit()
    {
        //hier komt niks
    }
}

