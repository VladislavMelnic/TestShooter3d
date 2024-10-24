using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    WeaponManager weaponManager;
    public void Initialize(WeaponManager weaponManager)
    {
        this.weaponManager = weaponManager; // ������������� WeaponManager ��� ������������� ����
    }


    void OnTriggerEnter(Collider other)
    {
        // ���������, ���� �� � ������� ��������� PlayerHealth
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        EnemyHP enemyHP = other.GetComponent<EnemyHP>();


        if (weaponManager == null)
        {
            weaponManager = GetComponent<WeaponManager>(); // ���� WeaponManager �� ��� �� �������
        }

        if (playerHealth != null)
        {
            // ������� ����
            playerHealth.TakeDamage(weaponManager.currentWeapon.damage);
        }

        if (enemyHP != null)
        {
            // ������� ����
            enemyHP.TakeDamage(weaponManager.currentWeapon.damage);
        }



        // ���������� ���� ����� ������������
        Destroy(gameObject);
    }
}
