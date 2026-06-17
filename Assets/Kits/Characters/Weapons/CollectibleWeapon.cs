using UnityEngine;

public class CollectibleWeapon : MonoBehaviour
{
    [SerializeField] WeaponData _weaponData;

    public WeaponData GetWeapon() { return _weaponData; }
}
