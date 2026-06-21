using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Content/Items/NewItem")]
public class ItemSO : ScriptableObject
{
    [Header("Basic Info")]
    public Sprite ItemIcon;
    public string itemName;
    public int price;

    [Header("Use Settings")]
    [SerializeField] private bool canBeUsed = true;
    [SerializeField] private bool consumeOnUse = true;
    [SerializeField] private List<ItemEffectSO> effects = new List<ItemEffectSO>();

    [Header("Use Audio")]
    [SerializeField] private AudioClip useClip;
    [SerializeField] private float useClipVolume = 1f;

    public bool CanBeUsed => canBeUsed;
    public bool ConsumeOnUse => consumeOnUse;

    public AudioClip UseClip => useClip;
    public float UseClipVolume => useClipVolume;

    public bool UseItem(GameObject user)
    {
        if (!canBeUsed)
        {
            Debug.Log($"{itemName} cannot be used.");
            return false;
        }

        if (effects == null || effects.Count == 0)
        {
            Debug.LogWarning($"{itemName} has no effects.");
            return false;
        }

        bool usedSuccessfully = false;

        for (int i = 0; i < effects.Count; i++)
        {
            ItemEffectSO effect = effects[i];

            if (effect == null)
                continue;

            if (effect.Use(user))
            {
                usedSuccessfully = true;
            }
        }

        return usedSuccessfully;
    }
}