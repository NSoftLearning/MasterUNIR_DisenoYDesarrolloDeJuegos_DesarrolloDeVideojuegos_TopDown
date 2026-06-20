using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform attackPosUp;
    [SerializeField] private Transform attackPosDown;
    [SerializeField] private Transform attackPosLeft;
    [SerializeField] private Transform attackPosRight;

    public event Action FinishedAttack;
    public event Action OnNewWeapon;

    private readonly List<WeaponData> weapons = new List<WeaponData>();

    private Weapon currentWeapon;
    private int currentWeaponIndex = -1;
    private bool isAttacking = false;

    public void SetWeapons(IReadOnlyList<WeaponData> weaponList)
    {
        weapons.Clear();

        if (weaponList == null)
            return;

        for (int i = 0; i < weaponList.Count; i++)
        {
            AddWeaponIfMissing(weaponList[i]);
        }
    }

    public void NewWeapon(GameObject newWeapon)
    {
        if (newWeapon == null)
            return;

        CollectibleWeapon collectibleWeapon = newWeapon.GetComponent<CollectibleWeapon>();

        if (collectibleWeapon == null)
            return;

        WeaponData weapon = collectibleWeapon.GetWeapon();

        if (weapon == null)
            return;

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddWeapon(weapon);
            InventoryManager.Instance.EquipWeapon(weapon);
        }
        else
        {
            AddWeaponIfMissing(weapon);
            EquipWeapon(weapon);
        }
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        if (weaponData == null)
            return;

        AddWeaponIfMissing(weaponData);

        ClearCurrentWeaponInstance();

        GameObject newWeapon = Instantiate(weaponData._weaponPrefab, transform);
        currentWeapon = newWeapon.GetComponent<Weapon>();

        if (currentWeapon != null)
        {
            currentWeapon.FinishedAttack += OnWeaponFinishedAttack;
        }

        currentWeaponIndex = GetWeaponIndex(weaponData);

        OnNewWeapon?.Invoke();
    }

    public void ClearWeapon()
    {
        ClearCurrentWeaponInstance();
        weapons.Clear();
        currentWeaponIndex = -1;
        isAttacking = false;
    }

    public void ChangeNextWeapon()
    {
        if (weapons.Count == 0)
            return;

        if (currentWeaponIndex == 0 && weapons.Count == 1)
            return;

        currentWeaponIndex++;

        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0;
        }

        WeaponData weaponAct = weapons[currentWeaponIndex];

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.EquipWeapon(weaponAct);
        }
        else
        {
            EquipWeapon(weaponAct);
        }
    }

    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    public void Attack(Direction direction)
    {
        if (currentWeapon == null)
            return;

        Transform attackPos = attackPosLeft;

        switch (direction)
        {
            case Direction.Right:
                attackPos = attackPosRight;
                break;

            case Direction.Up:
                attackPos = attackPosUp;
                break;

            case Direction.Down:
                attackPos = attackPosDown;
                break;

            case Direction.Left:
                attackPos = attackPosLeft;
                break;
        }

        currentWeapon.Attack(direction, attackPos);
        isAttacking = true;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public float GetNeededStamina()
    {
        if (currentWeapon == null)
            return 0;

        return currentWeapon.GetNeededStamina();
    }

    private void OnWeaponFinishedAttack()
    {
        isAttacking = false;
        FinishedAttack?.Invoke();
    }

    private void AddWeaponIfMissing(WeaponData weaponData)
    {
        if (weaponData == null)
            return;

        for (int i = 0; i < weapons.Count; i++)
        {
            WeaponData weapon = weapons[i];

            if (weapon == null)
                continue;

            if (weapon == weaponData)
                return;

            if (weapon._weaponName == weaponData._weaponName)
                return;
        }

        weapons.Add(weaponData);
    }

    private int GetWeaponIndex(WeaponData weaponData)
    {
        if (weaponData == null)
            return -1;

        for (int i = 0; i < weapons.Count; i++)
        {
            WeaponData weapon = weapons[i];

            if (weapon == null)
                continue;

            if (weapon == weaponData)
                return i;

            if (weapon._weaponName == weaponData._weaponName)
                return i;
        }

        return -1;
    }

    private void ClearCurrentWeaponInstance()
    {
        if (currentWeapon == null)
            return;

        currentWeapon.FinishedAttack -= OnWeaponFinishedAttack;
        currentWeapon.AutoDestroy();
        currentWeapon = null;
    }

    private void OnDestroy()
    {
        ClearCurrentWeaponInstance();
    }
}