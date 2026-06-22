using System;
using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{
    IDamageReceiver _damageReceiver;
    [SerializeField] GameObject _deadFX;

    private void Awake()
    {
        _damageReceiver = GetComponent<IDamageReceiver>();
    }
    private void OnEnable()
    {
        _damageReceiver.Died += ShowFeedbackDied;
    }

    private void ShowFeedbackDied()
    {
        Instantiate(_deadFX, transform.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        _damageReceiver.Died -= ShowFeedbackDied;
    }
}
