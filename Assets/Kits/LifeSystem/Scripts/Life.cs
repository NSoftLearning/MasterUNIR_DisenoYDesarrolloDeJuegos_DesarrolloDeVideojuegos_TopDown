using System;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [SerializeField] float startLife = 1;
    [SerializeField] float damagePerHit = .3f;


    public UnityEvent <float, float> onLifeChanged;
    public UnityEvent <float> onLifeDepleted;

    HurtCollider hurtCollider;
    private float currentLife;

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
}
