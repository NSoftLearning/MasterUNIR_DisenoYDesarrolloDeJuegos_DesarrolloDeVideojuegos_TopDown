using UnityEngine;

public class PlayerSpawnLevel : MonoBehaviour
{
    [Header("Spawn Level")]
    [SerializeField] Transform spawnPoint;
    private GameObject player;

    private void Awake()
    {
        spawnPoint = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Player o SpawnPoint no asignados en PlayerSpawnLevel.");
        }
    }
}

