using UnityEngine;

[CreateAssetMenu(fileName = "HealItemEffect", menuName = "Content/Items/Effects/Heal Effect")]
public class HealingEffectSO : ItemEffectSO
{
    [SerializeField] private int healAmount = 10;

    public override bool Use(GameObject user)
    {
        if (user == null)
        {
            Debug.LogWarning("No user assigned for item use.");
            return false;
        }

        Life life = user.GetComponent<Life>();

        if (life == null)
        {
            Debug.LogWarning("Life component not found on user.");
            return false;
        }

        return life.Heal(healAmount);
    }
}