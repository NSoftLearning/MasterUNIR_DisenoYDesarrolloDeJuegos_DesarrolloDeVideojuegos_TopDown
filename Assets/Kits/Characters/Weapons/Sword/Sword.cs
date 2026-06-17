using System;
using UnityEngine;
using UnityEngine.Events;

public class Sword : Weapon
{
    public override void Attack(Direction direction, Transform location)
    {
        base.Attack(direction, location);

        Animator anim = actAttack.GetComponent<Animator>();

        anim.SetTrigger("Attack");
    }
}
