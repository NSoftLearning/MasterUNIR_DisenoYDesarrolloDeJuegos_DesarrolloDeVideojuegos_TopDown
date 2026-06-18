using UnityEngine;

public class PressurePlate : MonoBehaviour, ITrap
{
    [SerializeField] GameObject _trapObj;

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
    }

    public void Deactivate()
    {
        anim.SetBool("Activated", false);
    }
}
