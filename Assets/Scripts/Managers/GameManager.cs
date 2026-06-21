using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Progression")]
    HashSet<int> completedLevels = new();

    private void Awake()
    {
       if (FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length > 1)
       {
            Destroy(gameObject);
            return;
       }
       else 
       {
            DontDestroyOnLoad(gameObject);
       }
    }

    public void CompleteLevel(int levelNumber)
    {
        completedLevels.Add(levelNumber);
    }

    public bool IsLevelCompleted(int levelNumber)
    {
        return completedLevels.Contains(levelNumber);
    }

    public void ResetProgress()
    {
        completedLevels.Clear();
    }

}
