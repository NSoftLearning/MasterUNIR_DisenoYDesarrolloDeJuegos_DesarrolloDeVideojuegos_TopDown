using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] Transform attackPosUp;
    [SerializeField] Transform attackPosDown;
    [SerializeField] Transform attackPosLeft;
    [SerializeField] Transform attackPosRight;

    public event Action FinishedAttack;
    public event Action OnNewWeapon;
    
    private List<WeaponData> weapons;
    private Weapon currentWeapon;
    private int currentWeaponIndex = -1;

    private void Awake()
    {
        weapons = new List<WeaponData>();
    }

    public void NewWeapon(GameObject newWeapon)
    {
        WeaponData weapon = newWeapon.GetComponent<CollectibleWeapon>().GetWeapon();

        for (int i = 0; i < weapons.Count; i++)
        {
            WeaponData act = weapons[i];
            if (act._weaponName == weapon._weaponName) return;
        }

        OnNewWeapon?.Invoke();

        weapons.Add(weapon);

        ChangeNextWeapon();
    }

    public void ChangeNextWeapon()
    {
        if (weapons.Count == 0) return; // Para cuando no hay arma
        if (currentWeaponIndex == 0 && weapons.Count == 1) return; // Para cuando solo hay un arma

        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons.Count) currentWeaponIndex = 0;

        if (currentWeapon != null) 
        {
            currentWeapon.FinishedAttack -= OnWeaponFinishedAttack;
            currentWeapon.AutoDestroy();
        }

        WeaponData weaponAct = weapons[currentWeaponIndex];

        GameObject newWeapon = Instantiate(weaponAct._weaponPrefab, transform);
        currentWeapon = newWeapon.GetComponent<Weapon>();

        currentWeapon.FinishedAttack += OnWeaponFinishedAttack;
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    bool isAttacking = false;
    public void Attack(Direction direction)
    {
        if (currentWeapon == null) return;

        Transform attackPos = attackPosLeft;
        switch (direction)
        {
            //case Direction.Left: attackPos = attackPosLeft; break;
            case Direction.Right: attackPos = attackPosRight; break;
            case Direction.Up: attackPos = attackPosUp; break;
            case Direction.Down: attackPos = attackPosDown; break;
        }

        currentWeapon.Attack(direction, attackPos);
        isAttacking = true;
    }

    public bool IsAttacking() { return isAttacking; }

    public float GetNeededStamina()
    {
        if (currentWeapon == null) return 0;
        return currentWeapon.GetNeededStamina();
    }

    private void OnWeaponFinishedAttack()
    {
        isAttacking = false;
        FinishedAttack?.Invoke();
    }

    private void OnDestroy()
    {
        if (currentWeapon != null)
        {
            currentWeapon.FinishedAttack -= OnWeaponFinishedAttack;
        }
    }
}
