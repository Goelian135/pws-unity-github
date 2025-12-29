using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    protected WeaponStateMachine machine;
    protected Collider2D hitbox;
    protected GameObject vfx;
    protected GameObject weapon;

    public WeaponState(WeaponStateMachine machine, Collider2D hitbox, GameObject vfx, GameObject weapon)
    {
        this.machine = machine;
        this.hitbox = hitbox;
        this.vfx = vfx;
        this.weapon = weapon;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
