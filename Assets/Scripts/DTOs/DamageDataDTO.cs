using System.Collections.Generic;
using UnityEngine;

public struct DamageDataDTO
{
    public int damageAmount;
    public List<DamageableTypeSO> validTargets;
    public Vector3 damageOrigin;
    public float damagePushForce;

    public DamageDataDTO(int damageAmount, List<DamageableTypeSO> validTargets, Vector3 damageOrigin, float damagePushForce)
    {
        this.damageAmount = damageAmount;
        this.validTargets = validTargets;
        this.damageOrigin = damageOrigin;
        this.damagePushForce = damagePushForce;
    }
}
