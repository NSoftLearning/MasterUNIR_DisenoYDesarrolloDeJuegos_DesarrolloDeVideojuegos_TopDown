using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    public UnityEvent onHitReceived;
    internal void NotifyHit(HitCollider hurtCollider)
    {
        onHitReceived.Invoke();
    }
}
