using UnityEngine;

[CreateAssetMenu(fileName = "HealItemEffect", menuName = "Content/Items/Effects/Heal Effect")]
public class HealingEffectSO : ItemEffectSO
{
    [SerializeField] private int healAmount = 3;

    public override bool ApplyEffect(GameObject user)
    {
        if (user == null)
        {
            Debug.LogWarning("Cannot use heal item. User is null.");
            return false;
        }

        Life life = user.GetComponent<Life>();

        if (life == null)
        {
            Debug.LogWarning("Cannot use heal item. Life component not found.");
            return false;
        }

        return life.Heal(healAmount);
    }
}