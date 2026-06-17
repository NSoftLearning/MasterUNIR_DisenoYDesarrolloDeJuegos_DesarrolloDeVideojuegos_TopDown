using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Content/Items/NewItem")]
public class ItemSO : ScriptableObject
{
    [Header("Basic Info")]
    public Sprite ItemIcon;
    public string itemName;
    public int price;

    [Header("Use")]
    [SerializeField] private bool canBeUsed = true;
    [SerializeField] private bool consumeOnUse = true;
    [SerializeField] private List<ItemEffectSO> effects = new List<ItemEffectSO>();

    public bool ConsumeOnUse => consumeOnUse;

    public bool UseItem(GameObject user)
    {
        if (!canBeUsed)
        {
            Debug.Log($"{itemName} cannot be used.");
            return false;
        }

        if (effects == null || effects.Count == 0)
        {
            Debug.LogWarning($"{itemName} has no item effects.");
            return false;
        }

        bool usedSuccessfully = false;

        foreach (ItemEffectSO effect in effects)
        {
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