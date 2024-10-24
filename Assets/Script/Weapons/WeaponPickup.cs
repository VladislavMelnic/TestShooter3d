using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponToGive; // Оружие, которое куб выдаёт
    public string playerTag = "Player"; // Тег игрока, чтобы убедиться, что взаимодействие происходит с ним

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что объект, который коснулся куба, имеет тег "Player"
        if (other.CompareTag(playerTag))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                // Выдаём оружие игроку
                weaponManager.AddWeapon(weaponToGive);

                // Destroy the pickup object after it's collected
                Destroy(gameObject);

                // Если нужно, можно удалить куб или сделать его неактивным после получения оружия
                // Destroy(gameObject); // Удаляем куб после получения оружия
            }
        }
    }
}
