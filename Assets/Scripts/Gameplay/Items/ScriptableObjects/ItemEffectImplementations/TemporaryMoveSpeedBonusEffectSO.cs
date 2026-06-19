using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryMoveSpeedBonusEffect", menuName = "Content/Items/Effects/Temporary Move Speed Bonus")]
public class TemporaryMoveSpeedBonusEffectSO : ItemEffectSO
{
    [SerializeField] private float moveSpeedMultiplier = 1.5f;
    [SerializeField] private float duration = 5f;

    public override bool ApplyEffect(GameObject user)
    {
        if (user == null)
        {
            Debug.LogWarning("Cannot use move speed bonus item. User is null.");
            return false;
        }

        CharacterTemporaryStats temporaryStats = user.GetComponent<CharacterTemporaryStats>();

        if (temporaryStats == null)
        {
            Debug.LogWarning("Cannot use move speed bonus item. PlayerTemporaryStats component not found.");
            return false;
        }

        temporaryStats.ApplyMoveSpeedBonus(moveSpeedMultiplier, duration);
        return true;
    }
}