using UnityEngine;
using UnityEngine.UI;

public class EnemyUiHP : MonoBehaviour

{ 
public Slider healthSlider; // Ссылка на ползунок здоровья
public EnemyHP enemyHealth; // Ссылка на компонент PlayerHealth

void Start()
{
    if (enemyHealth != null)
    {
            // Подписываемся на событие изменения здоровья
            enemyHealth.onHealthChanged += UpdateHealthBar;
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
    if (enemyHealth != null)
    {
            enemyHealth.onHealthChanged -= UpdateHealthBar;
    }
}
}