using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour, IDamageReceiver
{
    [Header("Life")]
    [SerializeField] private int startLife = 10;
    [SerializeField] private int currentLife;
    [SerializeField] bool _destroyOnDeath = true;

    [Header("Damage Type")]
    [SerializeField] private DamageableTypeSO type;

    [Header("Unity Events")]
    public UnityEvent<float, float> onLifeChanged = new UnityEvent<float, float>();
    public UnityEvent<float> onLifeDepleted = new UnityEvent<float>();

    public event Action Died;
    public event Action Damaged;
    public event Action<LifeChangedDTO> LifeChanged;

    private int inmunitySources = 0;
    private bool manualInmunityActive = false;
    private bool hasDied = false;

    public DamageableTypeSO Type => type;
    public bool IsInmune => inmunitySources > 0;
    public int CurrentLife => currentLife;
    public int MaxLife => startLife;

    private void Awake()
    {
        currentLife = startLife;
        hasDied = false;

        NotifyLifeChanged(0);
    }

    public bool TryToDealDamage(DamageDataDTO damageData)
    {
        if (currentLife <= 0 || hasDied)
            return false;

        if (IsInmune)
            return false;

        if (damageData.validTargets == null || !damageData.validTargets.Contains(type))
            return false;

        if (damageData.damageAmount <= 0)
            return false;

        currentLife -= damageData.damageAmount;

        if (currentLife < 0)
        {
            currentLife = 0;
        }

        NotifyLifeChanged(-damageData.damageAmount);
        Damaged?.Invoke();

        if (currentLife <= 0 && !hasDied)
        {
            hasDied = true;

            Died?.Invoke();
            onLifeDepleted?.Invoke(startLife);
            if (_destroyOnDeath) 
                Destroy(gameObject);
        }

        return true;
    }

    public bool Heal(int amount)
    {
        if (amount <= 0)
            return false;

        if (currentLife <= 0 || hasDied)
            return false;

        if (currentLife >= startLife)
        {
            Debug.Log("Life is already full.");
            return false;
        }

        int previousLife = currentLife;

        currentLife += amount;

        if (currentLife > startLife)
        {
            currentLife = startLife;
        }

        int realHealAmount = currentLife - previousLife;

        NotifyLifeChanged(realHealAmount);

        return true;
    }

    public void AddInmunitySource()
    {
        inmunitySources++;
    }

    public void RemoveInmunitySource()
    {
        inmunitySources--;

        if (inmunitySources < 0)
        {
            inmunitySources = 0;
        }
    }

    public void SetInmunity(bool inmune)
    {
        if (inmune)
        {
            if (manualInmunityActive)
                return;

            manualInmunityActive = true;
            AddInmunitySource();
        }
        else
        {
            if (!manualInmunityActive)
                return;

            manualInmunityActive = false;
            RemoveInmunitySource();
        }
    }

    private void NotifyLifeChanged(int deltaValue)
    {
        LifeChanged?.Invoke(
            new LifeChangedDTO
            {
                currentValue = currentLife,
                maxValue = startLife,
                deltaValue = deltaValue
            });

        onLifeChanged?.Invoke(currentLife, startLife);
    }

    public bool DamageIsCompatible(List<DamageableTypeSO> compatibleTargetTypes)
    {
        if (compatibleTargetTypes == null)
            return false;

        return compatibleTargetTypes.Contains(type);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}