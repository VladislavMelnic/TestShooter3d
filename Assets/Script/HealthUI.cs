//using UnityEngine;
//using UnityEngine.UI;

//public class HealthUI : MonoBehaviour
//{
//    public PlayerHealth playerHealth; // Ссылка на скрипт с HP
//    public Slider healthSlider;       // Ссылка на слайдер UI

//    void Start()
//    {
//        // Подписываемся на событие изменения здоровья
//        playerHealth.onHealthChanged += UpdateHealthBar;

//        // Инициализируем слайдер значениями здоровья
//        healthSlider.maxValue = playerHealth.maxHealth;
//        healthSlider.value = playerHealth.currentHealth;
//    }

//    // Обновляем значение слайдера
//    void UpdateHealthBar(int currentHealth, int maxHealth)
//    {
//        healthSlider.value = currentHealth;
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Ссылка на ползунок здоровья
    public PlayerHealth playerHealth; // Ссылка на компонент PlayerHealth

    void Start()
    {
        if (playerHealth != null)
        {
            // Подписываемся на событие изменения здоровья
            playerHealth.onHealthChanged += UpdateHealthBar;
        }
    }

    // Метод для обновления значения ползунка здоровья
    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthSlider.value = (float)currentHealth / maxHealth; // Устанавливаем значение ползунка
        Debug.Log("Health bar updated: " + healthSlider.value);
    }

    void OnDestroy()
    {
        // Отписываемся от события, чтобы избежать ошибок
        if (playerHealth != null)
        {
            playerHealth.onHealthChanged -= UpdateHealthBar;
        }
    }
}
