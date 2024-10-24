using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    WeaponManager weaponManager;
    public void Initialize(WeaponManager weaponManager)
    {
        this.weaponManager = weaponManager; // Устанавливаем WeaponManager при инициализации пули
    }


    void OnTriggerEnter(Collider other)
    {
        // Проверяем, есть ли у объекта компонент PlayerHealth
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        EnemyHP enemyHP = other.GetComponent<EnemyHP>();


        if (weaponManager == null)
        {
            weaponManager = GetComponent<WeaponManager>(); // Если WeaponManager на том же объекте
        }

        if (playerHealth != null)
        {
            // Наносим урон
            playerHealth.TakeDamage(weaponManager.currentWeapon.damage);
        }

        if (enemyHP != null)
        {
            // Наносим урон
            enemyHP.TakeDamage(weaponManager.currentWeapon.damage);
        }



        // Уничтожаем пулю после столкновения
        Destroy(gameObject);
    }
}
