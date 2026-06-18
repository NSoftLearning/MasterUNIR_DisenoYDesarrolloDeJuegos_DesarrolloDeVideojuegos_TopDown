using System.Collections.Generic;
using UnityEngine;

public class DamageReceiverTargetSelector 
{
    private List<DamageableTypeSO> _damageableTypesOfInterest;

    public DamageReceiverTargetSelector(List<DamageableTypeSO> damageableTypesOfInterest)
    {
        _damageableTypesOfInterest = damageableTypesOfInterest;
    }

    public bool TryGetTargetOfInterest(
        List<FoundTargetDTO<IDamageReceiver>> targetsFound,
        out FoundTargetDTO<IDamageReceiver> targetOfInterest)
    {
        foreach (FoundTargetDTO<IDamageReceiver> foundTarget in targetsFound)
        {
            if (foundTarget.target.DamageIsCompatible(_damageableTypesOfInterest))
            {
                targetOfInterest = foundTarget;
                return true;
            }
        }

        targetOfInterest = default;
        return false;
    }
}
