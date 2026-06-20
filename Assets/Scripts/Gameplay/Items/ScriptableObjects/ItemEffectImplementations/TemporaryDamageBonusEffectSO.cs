using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryDamageBonusEffect", menuName = "Content/Items/Effects/Temporary Damage Bonus")]
public class TemporaryDamageBonusEffectSO : ItemEffectSO
{
    [SerializeField] private float damageMultiplier = 1.5f;
    [SerializeField] private float duration = 5f;

    public override bool ApplyEffect(GameObject user)
    {
        if (user == null)
        {
            Debug.LogWarning("Cannot use damage bonus item. User is null.");
            return false;
        }

        CharacterTemporaryStats temporaryStats = user.GetComponent<CharacterTemporaryStats>();

        if (temporaryStats == null)
        {
            Debug.LogWarning("Cannot use damage bonus item. PlayerTemporaryStats component not found.");
            return false;
        }

        temporaryStats.ApplyDamageBonus(damageMultiplier, duration);
        return true;
    }
}