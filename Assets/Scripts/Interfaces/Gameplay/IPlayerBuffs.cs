using UnityEngine;

public interface IPlayerBuffs 
{
    void RestoreHealth (int healthToRestore);
    void IncreaseSpeed(float duration, float icreaseValue);
    void IncreaseAttackDamage(float duration, float icreaseValue);
    void TempInvulnerability(float duration);
}
