using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour, IDamageReceiver
{
    [SerializeField] int startLife = 10;
    [SerializeField] int damagePerHit = 3;
    [SerializeField] DamageableTypeSO type;

    public UnityEvent <float, float> onLifeChanged;
    public UnityEvent <float> onLifeDepleted;

    //HurtCollider hurtCollider;
   [SerializeField] private int currentLife;

    
    public event Action Died;
    public event Action<LifeChangedDTO> LifeChanged;

    public DamageableTypeSO Type => type;


    private void Awake()
    {
        currentLife = startLife;
        /*hurtCollider = GetComponent<HurtCollider>();
        hurtCollider.onHitReceived.AddListener(OnHitReceived);
        onLifeChanged.Invoke(currentLife, startLife);
        */
    }

  /*  private void OnHitReceived()
    {
        if (currentLife > 0)
        {
            currentLife -= damagePerHit;
            onLifeChanged.Invoke(currentLife, startLife);

            if (currentLife <= 0)
            {
                currentLife = 0;
                onLifeDepleted.Invoke(startLife);
            }
        }
    }
  */

    /*
    [ContextMenu (nameof(SimulateHitReceived))]
    public void SimulateHitReceived () 
    {
        OnHitReceived();
    }

    */
    public bool TryToDealDamage(DamageDataDTO damageData)
    {
        if (currentLife <= 0)
            return false;

        if (!damageData.validTargets.Contains(type))
            return false;


        currentLife -= damageData.damageAmount;

        LifeChanged?.Invoke(
            new LifeChangedDTO { 
                currentValue = currentLife,
                maxValue = startLife,
                deltaValue = damageData.damageAmount});

        if (currentLife <= 0)
            Died?.Invoke();

        return true;

    }

    /*public bool CanDamage(FoundTargetDTO<IDamageReceiver> candidateTargets)
    {
        //return targetTypes.Contains(type);
        return false;
    }*/


    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool Heal(int amount)
{
    if (amount <= 0)
        return false;

    if (currentLife >= startLife)
    {
        Debug.Log("Life is already full.");
        return false;
    }

    currentLife += amount;

    if (currentLife > startLife)
    {
        currentLife = startLife;
    }
        LifeChanged.Invoke(
            new LifeChangedDTO
            {
                currentValue = currentLife,
                maxValue = startLife,
                deltaValue = amount
            });
   // onLifeChanged.Invoke(currentLife, startLife);

    return true;
    }

    public bool DamageIsCompatible(List<DamageableTypeSO> compatibleTargetTypes)
    {
        if (compatibleTargetTypes.Contains(type))
            return true;
        return false;
    }
}
