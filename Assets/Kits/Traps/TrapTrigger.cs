using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] GameObject _trapObj;
    [SerializeField] bool _deactivateOnExit = true;

    ITrap trap;
    private void Awake()
    {
        trap = _trapObj.GetComponent<ITrap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trap.Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_deactivateOnExit && collision.CompareTag("Player"))
        {
            trap.Deactivate();
        }
    }
}
