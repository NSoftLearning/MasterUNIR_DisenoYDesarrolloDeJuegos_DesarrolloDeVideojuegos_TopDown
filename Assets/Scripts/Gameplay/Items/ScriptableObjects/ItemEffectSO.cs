using UnityEngine;

public abstract class ItemEffectSO : ScriptableObject
{
    public abstract bool Use(GameObject user);
}