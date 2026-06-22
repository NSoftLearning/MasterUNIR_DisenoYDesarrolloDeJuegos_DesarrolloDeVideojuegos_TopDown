using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("SO_GameProgress")]
    [SerializeField] private LevelProgressSO progress;

    public void CompleteLevel(int levelNumber)
    {
        progress.CompleteLevel(levelNumber);
    }

    public bool IsLevelCompleted(int levelNumber)
    {
        return progress.IsCompleted(levelNumber);
    }

    public void ResetProgress()
    {
        progress.ResetProgress();
    }

}
