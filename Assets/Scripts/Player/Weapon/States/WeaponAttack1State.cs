using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack1State : WeaponState
{
    public WeaponAttack1State(WeaponStateMachine machine, Collider2D hitbox, GameObject vfx, GameObject weapon) 
        : base(machine, hitbox, vfx, weapon)
    { }

    float attackDuration = 0.2f;
    private float timer;

    public Vector3 target1 = new Vector3(0.25f, 0.5f, 0f);
    public Vector3 target2 = new Vector3(-0.16f, 0.4f, 0f);

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public override void Enter()
    {
        base.Enter();

        timer = attackDuration;
        machine.hitbox.enabled = true;
        if (!vfx.activeSelf)
        {
            vfx.SetActive(true);
        }
        // Sla de originele positie en rotatie van het wapen op
        originalPosition = weapon.transform.localPosition;
        originalRotation = weapon.transform.localRotation;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            machine.ChangeState(machine.idleState);
            return;
        }

        // Simpele heen-en-weer zwaai animatie
        base.Update();
        if (timer > attackDuration / 2)
        {
            // Eerste helft van de aanval
            weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, target1, Time.deltaTime * 10f);
            weapon.transform.localRotation = Quaternion.Lerp(weapon.transform.localRotation, Quaternion.Euler(0f, 0f, -85f), Time.deltaTime * 10f);
            weapon.transform.localScale =  Vector3.Lerp(weapon.transform.localScale, new Vector3(1.0f, -1.0f, 1.0f), Time.deltaTime * 10f);
        }
        else
        {
            // Tweede helft van de aanval
            weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, target2, Time.deltaTime * 10f);
            weapon.transform.localRotation = Quaternion.Lerp(weapon.transform.localRotation, Quaternion.Euler(0f, 0f, -280f), Time.deltaTime * 10f);
        }
    }

    public override void Exit()
    {
        machine.hitbox.enabled = false;
        vfx.SetActive(false);
        // Reset de positie en rotatie van het wapen
        weapon.transform.localPosition = originalPosition;
        weapon.transform.localRotation = originalRotation;
        weapon.transform.localScale = new Vector3(1f, 1f, 1f);
        base.Exit();
    }
}
