using UnityEngine;

public class Axe : Weapon
{
    public override void Attack(Direction direction, Transform location)
    {
        base.Attack(direction, location);

        Animator anim = actAttack.GetComponent<Animator>();

        anim.SetTrigger("Attack");
    }
}
