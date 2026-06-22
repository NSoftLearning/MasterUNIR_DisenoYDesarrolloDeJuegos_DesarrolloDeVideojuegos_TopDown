using UnityEngine;

public class EnemyDeadFeedback : MonoBehaviour
{
    [SerializeField] GameObject _root;
   public void SelfDestroy ()
    {
        Destroy(_root);
    }
}
