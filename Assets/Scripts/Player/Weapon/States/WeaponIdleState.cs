using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIdleState : WeaponState
{
    public WeaponIdleState(WeaponStateMachine machine, Collider2D hitbox, GameObject vfx, GameObject weapon) 
        : base(machine, hitbox, vfx, weapon)
    {    }

    public override void Enter()
    {
        // Zet de hitbox uit wanneer het wapen in idle is
        machine.hitbox.enabled = false;
        vfx.SetActive(false);
    }

    public override void Update()
    {
        if (InputManager.Instance.GetKeyDown("Attack"))
        {
            machine.ChangeState(machine.attack1State);
        }
    }
}
