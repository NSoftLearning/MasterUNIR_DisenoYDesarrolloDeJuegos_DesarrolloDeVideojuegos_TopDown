using UnityEngine;

public class SpikeTrap : MonoBehaviour, ITrap
{
    [SerializeField] float _timeToDeactivate = 1f;
    [SerializeField] float _timeToCanActivateAgain = 2f;

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
    }

    public void Deactivate()
    {
        anim.SetBool("Activated", false);
    }

    private void SetCanActivate()
    {
        canActivate = true;
    }
}
