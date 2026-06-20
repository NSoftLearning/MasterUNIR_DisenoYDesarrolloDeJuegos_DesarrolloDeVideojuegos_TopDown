using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        // Imprescindible para 2D con NavMeshPlus
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (_target == null)
            return;

        if (!_agent.isOnNavMesh)
        {
            Debug.LogWarning($"{name} is not on the NavMesh.");
            return;
        }

        _agent.SetDestination(_target.position);
    }
}
