using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour, IDamageReceiver
{
    [SerializeField] float startLife = 1;
    [SerializeField] float damagePerHit = .3f;
    [SerializeField] DamageableTypeSO type;

    public UnityEvent <float, float> onLifeChanged;
    public UnityEvent <float> onLifeDepleted;

    HurtCollider hurtCollider;
    private float currentLife;

    public event Action<DamageDataDTO> DamageReceived;

    public DamageableTypeSO Type => type;


    private void Awake()
    {
        currentLife = startLife;
        hurtCollider = GetComponent<HurtCollider>();
        hurtCollider.onHitReceived.AddListener(OnHitReceived);
    }

    private void OnHitReceived()
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


    [ContextMenu (nameof(SimulateHitReceived))]
    public void SimulateHitReceived () 
    {
        OnHitReceived();
    }

    public bool TryToDealDamage(DamageDataDTO damageData)
    {
        throw new NotImplementedException();
    }

    public bool CanDamage(List<DamageableTypeSO> targetTypes)
    {
        throw new NotImplementedException();
    }
}
