using System;
using UnityEngine;

public interface IWeapon
{
    void Attack(Direction direction, Transform location);
    float GetNeededStamina();
    event Action FinishedAttack;
    void AutoDestroy();
}
