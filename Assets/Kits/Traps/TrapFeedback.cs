using UnityEngine;

public class TrapFeedback : MonoBehaviour
{
    ITrap trap;
    private void Awake()
    {
        trap = GetComponent<ITrap>();
    }

    private void OnEnable()
    {
        trap.OnActivate += OnActivate;
        trap.OnDeactivate += OnDeactivate;
    }

    private void OnActivate()
    {

    }

    private void OnDeactivate()
    {

    }

    private void OnDisable()
    {
        trap.OnActivate -= OnActivate;
        trap.OnDeactivate -= OnDeactivate;
    }
}
