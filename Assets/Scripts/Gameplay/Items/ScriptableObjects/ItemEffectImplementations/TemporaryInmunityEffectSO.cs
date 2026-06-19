using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryInvulnerabilityEffect", menuName = "Content/Items/Effects/Temporary Invulnerability")]
public class TemporaryInvulnerabilityEffectSO : ItemEffectSO
{
    [SerializeField] private float duration = 3f;

    public override bool ApplyEffect(GameObject user)
    {
        if (user == null)
        {
            Debug.LogWarning("Cannot use invulnerability item. User is null.");
            return false;
        }

        CharacterTemporaryStats temporaryStats = user.GetComponent<CharacterTemporaryStats>();

        if (temporaryStats == null)
        {
            Debug.LogWarning("Cannot use invulnerability item. PlayerTemporaryStats component not found.");
            return false;
        }

        temporaryStats.ApplyInvulnerability(duration);
        return true;
    }
}