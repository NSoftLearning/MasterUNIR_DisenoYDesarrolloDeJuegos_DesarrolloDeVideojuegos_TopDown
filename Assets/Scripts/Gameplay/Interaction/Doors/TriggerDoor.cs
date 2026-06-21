using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [Header("Level Transition Trigger")]
    [SerializeField] public bool doorInGame = false;
    [SerializeField] public int levelComplete;
    [SerializeField] public string nextSceneName;
    [SerializeField] bool saveInventory = true;

    GameManager gameManager;
    ScenesManager scenesManager;
    Collider2D col;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        scenesManager = FindAnyObjectByType<ScenesManager>();
        col = GetComponent<Collider2D>();
        col.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && doorInGame)
        {
            if (saveInventory)
            {
                ComponentLocatorService.Components.InventoryManager.SaveRuntimeToInventorySO();
            }
            col.enabled = false;
            gameManager.CompleteLevel(levelComplete);
            scenesManager.CallFadeOut_LoadScene(nextSceneName);
        }
    }
}

