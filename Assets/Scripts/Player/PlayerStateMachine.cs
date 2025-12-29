using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    //huidige state
    public PlayerState Currentstate { get; private set; }

    //alle mogelijke states
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }

    // referenties naar andere scripts
    public CharacterController2D controller;
    public PlayerInput input;
    public Rigidbody2D rb;


    void Awake()
    {
        //componenten initializeren
        controller = GetComponent<CharacterController2D>();
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();

        //states initializeren
        IdleState = new PlayerIdleState(this, controller, input, rb);
        RunState = new PlayerRunState(this, controller, input, rb);
        JumpState = new PlayerJumpState(this, controller, input, rb);
        FallState = new PlayerFallState(this, controller, input, rb);
        DashState = new PlayerDashState(this, controller, input, rb);
        AttackState = new PlayerAttackState(this, controller, input, rb);
    }

    private void Start()
    {
        //begin in idle state
        ChangeState(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        Currentstate.Update();
    }

    void FixedUpdate()
    {
        Currentstate.FixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        if (Currentstate != null)
            Currentstate.Exit();

        Currentstate = newState;
        Currentstate.Enter();
    }
}
