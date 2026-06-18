using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageReceiver 
{
    DamageableTypeSO Type { get; }
    event Action<LifeChangedDTO> LifeChanged;
    event Action Died;
    bool TryToDealDamage(DamageDataDTO damageData);
    bool CanDamage(List<DamageableTypeSO> targetTypes);
    Vector3 GetPosition();
}
