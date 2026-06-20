using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [Header("Level Transition Trigger")]
    public bool doorInGame = false;
    public int levelComplete;
    public string nextSceneName;

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
            col.enabled = false;
            gameManager.CompleteLevel(levelComplete);
            scenesManager.CallFadeOut_LoadScene(nextSceneName);
        }
    }
}

