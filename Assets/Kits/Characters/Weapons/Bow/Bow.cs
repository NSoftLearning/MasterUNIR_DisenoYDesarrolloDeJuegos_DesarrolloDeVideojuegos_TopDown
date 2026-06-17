using UnityEngine;

public class Bow : Weapon
{
    public override void Attack(Direction direction, Transform location)
    {
        base.Attack(direction, location);

        Projectile proj = actAttack.GetComponent<Projectile>();

        proj.SetDirection(direction);
    }
}
