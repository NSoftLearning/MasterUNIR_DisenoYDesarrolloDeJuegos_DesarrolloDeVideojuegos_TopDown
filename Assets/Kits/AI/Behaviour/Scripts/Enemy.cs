using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State
    {
        Guarding,
        Wandering,
        Patrolling,
        Seeking,
        Attacking,
        BeingHit,
        Dead
    }
    Sight sight;
    CustomCharacterController characterController;
    private void Awake()
    {
        sight = GetComponent<Sight>();
        characterController = GetComponent<CustomCharacterController>();
    }

    State currentState = State.Guarding;
    private void Update()
    {
        switch (currentState)
        {
            case State.Guarding:
                if (sight.visiblesInSight.Count > 0)
                {
                    currentState = State.Seeking;
                }
                else
                    characterController.SetRawMovement(Vector2.zero);
                    break;
            case State.Wandering:
                break;
            case State.Patrolling:
                break;
            case State.Seeking:
                if (sight.visiblesInSight.Count <= 0)
                {
                    currentState = State.Guarding;
                }
                else
                {
                    characterController.SetRawMovement((sight.visiblesInSight[0].GetTransform().position - transform.position).normalized);
                }
                break;
            case State.Attacking:
                break;
            case State.BeingHit:
                break;
            case State.Dead:
                break;
        }
    }
}
