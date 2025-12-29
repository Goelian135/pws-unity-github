using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine machine;
    protected CharacterController2D controller;
    protected PlayerInput input;
    protected Rigidbody2D rb;

    protected PlayerState(PlayerStateMachine machine, CharacterController2D controller, PlayerInput input, Rigidbody2D rb)
    {
        this.machine = machine;
        this.controller = controller;
        this.input = input;
        this.rb = rb;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}
