using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;           // Имя оружия
    public int damage;                  // Урон от оружия
    public int bulletsPerMagazine;      // Количество патронов в обойме
    public int totalBullets;            // Общее количество патронов
    public GameObject weaponPrefab;     // Префаб для скина оружия
    public bool isAutomatic; // Автоматический режим
    public float fireRate; // Задержка между выстрелами (для автоматического оружия)
    public Sprite imageSprite;

    public ParticleSystem muzzleFlash; // Это ваш ParticleSystem для выстрела

    [HideInInspector]
    public int currentBullets;          // Текущее количество патронов в обойме

    private void OnEnable()
    {
        // Инициализация текущего количества патронов
        currentBullets = bulletsPerMagazine;
    }

    // Метод для стрельбы
    public bool Shoot()
    {
        if (currentBullets > 0)
        {
            // Логика выстрела (например, создаем снаряд или Raycast)
            currentBullets--; // Уменьшаем количество патронов
            // Запуск эффекта выстрела
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
            return true; // Возвращаем true, если выстрел выполнен
        }
        else
        {
            Debug.Log("Нет патронов в магазине!");
            return false; // Нет патронов для выстрела
        }
    }

    // Метод для перезарядки
    public void Reload()
    {
        if (totalBullets > 0 && currentBullets < bulletsPerMagazine)
        {
            int bulletsNeeded = bulletsPerMagazine - currentBullets; // Сколько патронов нужно для полной обоймы
            int bulletsToReload = Mathf.Min(bulletsNeeded, totalBullets); // Не больше, чем есть в запасе

            currentBullets += bulletsToReload; // Обновляем текущее количество патронов
            totalBullets -= bulletsToReload;    // Уменьшаем общее количество патронов
            Debug.Log("Перезарядка: добавлено " + bulletsToReload + " патронов. Осталось: " + totalBullets);
        }
        else
        {
            Debug.Log("Нет патронов для перезарядки или магазин полный!");
        }
    }
}
