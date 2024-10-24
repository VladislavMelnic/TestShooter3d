using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;       // ������� ������
    public Transform weaponHolder;     // �����, ��� ����� ������������ ������ (��������, � ����� ���������)
    private GameObject activeWeapon;   // �������� ��������� ������� ������

    public List<Weapon> availableWeapons = new List<Weapon>();
    private int currentWeaponIndex = 0;    // ������ �������� ������

    void Start()
    {
        if (availableWeapons.Count > 0)
        {
            EquipWeapon(availableWeapons[currentWeaponIndex]); // ��������� ������ ������
        }
    }

    // ����� ��� ���������� ������ ������
    //public void EquipWeapon(Weapon newWeapon)
    //{
    //    if (activeWeapon != null)
    //    {
    //        Destroy(activeWeapon); // ������� ���������� ������
    //    }

    //    currentWeapon = newWeapon;

    //    // ���������� ������ ������ �� ������
    //    if (currentWeapon != null && currentWeapon.weaponPrefab != null)
    //    {
    //        activeWeapon = Instantiate(currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
    //        activeWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // ������� �� ��� Y
    //    }

    //    // ������� ������� � ����� ���������� ��������
    //    Debug.Log($"Equipped {newWeapon.weaponName}, Damage: {newWeapon.damage}");
    //}

    //public void EquipWeapon(Weapon newWeapon)
    //{
    //    if (activeWeapon != null)
    //    {
    //        Destroy(activeWeapon); // ������� ���������� ������
    //    }

    //    currentWeapon = newWeapon;

    //    // ���������� ������ ������ �� ������
    //    if (currentWeapon != null && currentWeapon.weaponPrefab != null)
    //    {
    //        // ������������ ������ � ����������� ��� � WeaponHolder
    //        activeWeapon = Instantiate(currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);

    //        // ���������, ��� ��������� ������� � ���������� ����������
    //        activeWeapon.transform.localPosition = Vector3.zero; // � ������������ � �������
    //        activeWeapon.transform.localRotation = Quaternion.identity; // ������ ����� ��������� �� ���������� ��������� ����
    //    }

    //    // ������� ������� � ����� ���������� ��������
    //    Debug.Log($"Equipped {newWeapon.weaponName}, Damage: {newWeapon.damage}");
    //}


    public void EquipWeapon(Weapon newWeapon)
    {
        if (activeWeapon != null)
        {
            Destroy(activeWeapon); // ������� ���������� ������
        }

        currentWeapon = newWeapon;

        // ���������� ������ ������ �� ������
        if (currentWeapon != null && currentWeapon.weaponPrefab != null)
        {
            // ������� ������
            activeWeapon = Instantiate(currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);

            // ������������� ��������� ������� � ����������, ���� ����������
            activeWeapon.transform.localPosition = Vector3.zero; // ���������, ��� ������ ��������� � ������ WeaponHolder
            activeWeapon.transform.localRotation = Quaternion.identity; // ���������, ��� ������ ����� ���������� ����������
        }

        // ������� ������� � ����� ���������� ��������
        Debug.Log($"Equipped {newWeapon.weaponName}, Damage: {newWeapon.damage}");
    }


    // ����� ��� ���������� ������ ������ � ������
    public void AddWeapon(Weapon newWeapon)
    {
        if (!availableWeapons.Contains(newWeapon))
        {
            availableWeapons.Add(newWeapon);
            EquipWeapon(newWeapon);  // ������������� ����������� ����� �������
            Debug.Log($"Picked up new weapon: {newWeapon.weaponName}");
        }
    }

    // �������� ������� ������
    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void SwitchWeapon(int direction)
    {
        if (availableWeapons.Count == 0) return; // ���� ��� ���������� ������

        // �������� ������ �������� ������
        currentWeaponIndex += direction;

        // ���������, �� ����� �� �� �� ������� ������
        if (currentWeaponIndex < 0)
            currentWeaponIndex = availableWeapons.Count - 1; // ������� � ���������� ��������
        else if (currentWeaponIndex >= availableWeapons.Count)
            currentWeaponIndex = 0; // ������� � ������� ��������

        EquipWeapon(availableWeapons[currentWeaponIndex]); // ��������� ����� ������
    }
}
