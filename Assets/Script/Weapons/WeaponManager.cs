using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon;       // Текущее оружие
    public Transform weaponHolder;     // Место, где будет отображаться оружие (например, в руках персонажа)
    private GameObject activeWeapon;   // Активный экземпляр префаба оружия

    public List<Weapon> availableWeapons = new List<Weapon>();
    private int currentWeaponIndex = 0;    // Индекс текущего оружия

    void Start()
    {
        if (availableWeapons.Count > 0)
        {
            EquipWeapon(availableWeapons[currentWeaponIndex]); // Экипируем первое оружие
        }
    }

    // Метод для экипировки нового оружия
    //public void EquipWeapon(Weapon newWeapon)
    //{
    //    if (activeWeapon != null)
    //    {
    //        Destroy(activeWeapon); // Удаляем предыдущее оружие
    //    }

    //    currentWeapon = newWeapon;

    //    // Отображаем префаб оружия на экране
    //    if (currentWeapon != null && currentWeapon.weaponPrefab != null)
    //    {
    //        activeWeapon = Instantiate(currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
    //        activeWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // Поворот по оси Y
    //    }

    //    // Текущие патроны и общее количество патронов
    //    Debug.Log($"Equipped {newWeapon.weaponName}, Damage: {newWeapon.damage}");
    //}

    //public void EquipWeapon(Weapon newWeapon)
    //{
    //    if (activeWeapon != null)
    //    {
    //        Destroy(activeWeapon); // Удаляем предыдущее оружие
    //    }

    //    currentWeapon = newWeapon;

    //    // Отображаем префаб оружия на экране
    //    if (currentWeapon != null && currentWeapon.weaponPrefab != null)
    //    {
    //        // Инстанцируем оружие и прикрепляем его к WeaponHolder
    //        activeWeapon = Instantiate(currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);

    //        // Убедитесь, что локальная позиция и ориентация правильные
    //        activeWeapon.transform.localPosition = Vector3.zero; // В соответствии с оружием
    //        activeWeapon.transform.localRotation = Quaternion.identity; // Оружие будет следовать за движениями оружейной руки
    //    }

    //    // Текущие патроны и общее количество патронов
    //    Debug.Log($"Equipped {newWeapon.weaponName}, Damage: {newWeapon.damage}");
    //}


    public void EquipWeapon(Weapon newWeapon)
    {
        if (activeWeapon != null)
        {
            Destroy(activeWeapon); // Удаляем предыдущее оружие
        }

        currentWeapon = newWeapon;

        // Отображаем префаб оружия на экране
        if (currentWeapon != null && currentWeapon.weaponPrefab != null)
        {
            // Спавним оружие
            activeWeapon = Instantiate(currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);

            // Устанавливаем локальную позицию и ориентацию, если необходимо
            activeWeapon.transform.localPosition = Vector3.zero; // Убедитесь, что оружие спавнится в центре WeaponHolder
            activeWeapon.transform.localRotation = Quaternion.identity; // Убедитесь, что оружие имеет правильную ориентацию
        }

        // Текущие патроны и общее количество патронов
        Debug.Log($"Equipped {newWeapon.weaponName}, Damage: {newWeapon.damage}");
    }


    // Метод для добавления нового оружия в список
    public void AddWeapon(Weapon newWeapon)
    {
        if (!availableWeapons.Contains(newWeapon))
        {
            availableWeapons.Add(newWeapon);
            EquipWeapon(newWeapon);  // Автоматически экипировать после подбора
            Debug.Log($"Picked up new weapon: {newWeapon.weaponName}");
        }
    }

    // Получаем текущее оружие
    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void SwitchWeapon(int direction)
    {
        if (availableWeapons.Count == 0) return; // Если нет доступного оружия

        // Изменяем индекс текущего оружия
        currentWeaponIndex += direction;

        // Проверяем, не вышли ли мы за границы списка
        if (currentWeaponIndex < 0)
            currentWeaponIndex = availableWeapons.Count - 1; // Переход к последнему элементу
        else if (currentWeaponIndex >= availableWeapons.Count)
            currentWeaponIndex = 0; // Возврат к первому элементу

        EquipWeapon(availableWeapons[currentWeaponIndex]); // Экипируем новое оружие
    }
}
