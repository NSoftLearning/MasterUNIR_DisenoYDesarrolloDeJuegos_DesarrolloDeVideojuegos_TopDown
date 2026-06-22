using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageOnCollision : MonoBehaviour
{
    private Collider2D _ignoredCollider;
    public event Action HitSomething;
    [SerializeField] int _damageToDeal;
    [SerializeField] List <DamageableTypeSO>  validTargets;
    [SerializeField] GameObject _root;
    [SerializeField] LayerMask _relevantLayers;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Initialize (Collider2D ignoredCollider)
    {

        _ignoredCollider = ignoredCollider;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == _ignoredCollider)
            return;
        if ((_relevantLayers.value & (1 << collision.gameObject.layer)) == 0)
                return;

        var damageReceiver = collision.GetComponent<IDamageReceiver>();

        if (damageReceiver != null)
        {
            if (damageReceiver.TryToDealDamage(new DamageDataDTO(_damageToDeal, validTargets, transform.position, 0)))
            {
                HitSomething?.Invoke();
                Destroy(_root);                
            }
        }
        else
        {
            HitSomething?.Invoke();
            Destroy(_root);
        }
        
            
        
        }

}
