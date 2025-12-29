using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateMachine : MonoBehaviour
{
    //huidige state
    public WeaponState currentState { get; private set; }

    //alle mogelijke states
    public WeaponIdleState idleState { get; private set; }
    public WeaponAttack1State attack1State { get; private set; }
    //public WeaponAttack2State attack2State;

    // referenties naar andere dingen
    public Collider2D hitbox;
    public GameObject vfx;
    public GameObject weapon;

    private void Awake()
    {
        //states initializeren
        idleState = new WeaponIdleState(this, hitbox, vfx, weapon);
        attack1State = new WeaponAttack1State(this, hitbox, vfx, weapon);
        //attack2State = GetComponent<WeaponAttack2State>();
    }

    void Start()
    {
        //begin in idle state
        ChangeState(idleState);
    }

    void Update()
    {
        currentState.Update();
    }

    public void ChangeState(WeaponState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Attack1()
    {
        ChangeState(attack1State);
    }
}

