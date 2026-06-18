using System;
using UnityEngine;

public class SpikeTrap : MonoBehaviour, ITrap
{
    [SerializeField] float _timeToDeactivate = 1f;
    [SerializeField] float _timeToCanActivateAgain = 2f;

    public event Action OnActivate;
    public event Action OnDeactivate;

    bool canActivate = true;
    BoxCollider2D boxC;
    Animator anim;
    private void Awake()
    {
        boxC = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        canActivate = true;
        boxC.enabled = false;
    }

    public void Activate()
    {
        if (!canActivate) return;

        canActivate = false;

        anim.SetBool("Activated", true);

        boxC.enabled = true;

        Invoke(nameof(Deactivate), _timeToDeactivate);

        Invoke(nameof(SetCanActivate), _timeToCanActivateAgain);

        OnActivate.Invoke();
    }

    public void Deactivate()
    {
        anim.SetBool("Activated", false);

        OnDeactivate.Invoke();
    }

    private void SetCanActivate()
    {
        canActivate = true;
    }
}
