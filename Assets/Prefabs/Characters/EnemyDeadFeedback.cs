using UnityEngine;

public class EnemyDeadFeedback : MonoBehaviour
{

   public void SelfDestroy ()
    {
        Destroy(gameObject);
    }
}
