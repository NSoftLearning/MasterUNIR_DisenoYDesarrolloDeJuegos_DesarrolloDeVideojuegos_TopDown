using UnityEngine;

public interface IDamageReceiver 
{
    //rellenar
    //bool CanDamage(DamageType type);
    void ApplyDamage(int damage /*, DamageType type*/);
}
