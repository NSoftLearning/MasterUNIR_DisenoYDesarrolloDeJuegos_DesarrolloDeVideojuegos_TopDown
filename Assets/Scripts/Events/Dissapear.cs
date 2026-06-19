using UnityEngine;

public class Dissapear : MonoBehaviour
{
    [SerializeField] float _timeToDissapear = 1f;

    private void Awake()
    {
        Destroy(gameObject, _timeToDissapear);
    }
}
