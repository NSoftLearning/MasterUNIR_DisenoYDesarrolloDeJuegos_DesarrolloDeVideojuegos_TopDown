using System;
using UnityEngine;

public class EnemyFeedback : MonoBehaviour
{
    [SerializeField] GameObject _deadFX;

    public void ShowFeedbackDied()
    {
        Instantiate(_deadFX, transform.position, Quaternion.identity);
    }


}
