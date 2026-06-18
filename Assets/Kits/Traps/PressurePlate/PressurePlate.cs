using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour, ITrap
{
    [SerializeField] GameObject _trapObj;

    public event Action OnActivate;
    public event Action OnDeactivate;

    Animator anim;

    ITrap trap;
    private void Awake()
    {
        anim = GetComponent<Animator>();

        trap = _trapObj.GetComponent<ITrap>();
    }

    public void Activate()
    {
        anim.SetBool("Activated", true);

        trap.Activate();

        OnActivate.Invoke();
    }

    public void Deactivate()
    {
        anim.SetBool("Activated", false);

        OnDeactivate.Invoke();
    }
}
