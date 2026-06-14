using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageReceiver 
{
    DamageableTypeSO Type { get; }
    event Action<DamageDataDTO> DamageReceived;
    bool TryToDealDamage(DamageDataDTO damageData);
    bool CanDamage(List<DamageableTypeSO> targetTypes);
}
