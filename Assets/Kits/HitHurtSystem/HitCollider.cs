using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitCollider : MonoBehaviour
{
    [SerializeField] List<DamageableTypeSO> _validDamageables;
    [SerializeField] int _damageAmount;
    [SerializeField] float _pushForce;
    DamageDataDTO damageDataDTO;
    
    private void Start()
    {
        Debug.Log("hit collider start");

        damageDataDTO = new DamageDataDTO(_damageAmount, _validDamageables, transform.position, _pushForce);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //HurtCollider hit = collision.GetComponent<HurtCollider>();
        //hit?.NotifyHit(this);
        IDamageReceiver damageReceiver = collision.GetComponent<IDamageReceiver>();
        damageReceiver.TryToDealDamage(damageDataDTO);



    }

}
