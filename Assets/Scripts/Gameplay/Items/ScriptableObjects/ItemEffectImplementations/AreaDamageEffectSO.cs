using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "AreaDamageEffect", menuName = "Content/Items/Effects/Area Damage Effect")]
public class AreaDamageEffectSO : ItemEffectSO
{
    [SerializeField] private DamageDataDTO damageData;
    [SerializeField] private float radius = 3f;
    public ITargetFinder<IDamageReceiver, CircleTargetFinderQuerySettings> targetFinder;
    public CircleTargetFinderQuerySettings targetFinderQuerySettings;
    private bool hasCorrectlyDamagedAnyTarget;
    private void Awake()
    {
        hasCorrectlyDamagedAnyTarget = false;
    }
    public override bool Use(GameObject user)
    {
        if (user == null)
        {
            Debug.LogWarning("No user assigned for item use.");
            return false;
        }
        targetFinder = new TargetFinder_CircularDIstance<IDamageReceiver>();
        targetFinderQuerySettings = new CircleTargetFinderQuerySettings(user.transform.position, radius);
        List<FoundTargetDTO<IDamageReceiver>> resultList = targetFinder.FindTargets(targetFinderQuerySettings);
        
        foreach(FoundTargetDTO<IDamageReceiver> foundTarget in resultList)
        {
            if(foundTarget != null)
            {
                if(foundTarget.target != null)
                {
                    if(foundTarget.target.TryToDealDamage(damageData))
                        hasCorrectlyDamagedAnyTarget = true;
                    
                }
            }
        }

        return hasCorrectlyDamagedAnyTarget;
    }
}