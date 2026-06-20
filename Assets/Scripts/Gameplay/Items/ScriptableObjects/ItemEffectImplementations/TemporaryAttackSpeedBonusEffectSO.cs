using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryAttackSpeedBonusEffect", menuName = "Content/Items/Effects/Temporary Attack Speed Bonus")]
public class TemporaryAttackSpeedBonusEffectSO : ItemEffectSO
{
    [SerializeField] private float attackSpeedMultiplier = 1.5f;
    [SerializeField] private float duration = 5f;

    public override bool ApplyEffect(GameObject user)
    {
        if (user == null)
        {
            Debug.LogWarning("Cannot use attack speed bonus item. User is null.");
            return false;
        }

        CharacterTemporaryStats temporaryStats = user.GetComponent<CharacterTemporaryStats>();

        if (temporaryStats == null)
        {
            Debug.LogWarning("Cannot use attack speed bonus item. PlayerTemporaryStats component not found.");
            return false;
        }

        temporaryStats.ApplyAttackSpeedBonus(attackSpeedMultiplier, duration);
        return true;
    }
}