using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageOnCollision : MonoBehaviour
{
    private Collider2D _ignoredCollider;
    UnityEvent HitSomething;
    [SerializeField] int _damageToDeal;
    [SerializeField] List <DamageableTypeSO>  validTargets;
    
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

        var damageReceiver = collision.GetComponent<IDamageReceiver>();
        if (damageReceiver != null)
            damageReceiver.TryToDealDamage(new DamageDataDTO(_damageToDeal, validTargets, transform.position, 0));
            
        HitSomething?.Invoke();
        Destroy(transform.root.gameObject);
        
        }

}
