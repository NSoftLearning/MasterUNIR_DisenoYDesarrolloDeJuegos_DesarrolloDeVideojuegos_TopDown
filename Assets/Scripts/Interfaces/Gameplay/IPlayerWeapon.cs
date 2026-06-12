using UnityEngine;

public interface IPlayerWeapon
{
    bool IsReadyForAttack {  get; }
    bool TryToPerformAttack();
}
