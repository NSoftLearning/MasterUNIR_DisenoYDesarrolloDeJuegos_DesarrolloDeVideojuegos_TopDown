using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level Progress")]
public class LevelProgressSO : ScriptableObject
{
    public List<int> completedLevels = new();

    public void CompleteLevel(int level)
    {
        if (!completedLevels.Contains(level))
            completedLevels.Add(level);
    }

    public bool IsCompleted(int level)
    {
        return completedLevels.Contains(level);
    }

    public void ResetProgress()
    {
        completedLevels.Clear();
    }
}