using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Максимальное здоровье
    public int currentHealth;   // Текущее здоровье


    public GameManager gameManager; // Ссылка на GameManager

    // Событие вызова, когда персонаж получает урон
    public delegate void OnHealthChanged(int currentHealth, int maxHealth);
    public event OnHealthChanged onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth; // Устанавливаем начальное здоровье
    }

    // Функция для нанесения урона персонажу
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Ограничиваем здоровье минимумом
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Вызываем событие, чтобы обновить отображение HP на экране
        if (onHealthChanged != null)
        {

            Debug.Log(currentHealth);
            Debug.Log(maxHealth);
            onHealthChanged(currentHealth, maxHealth);
        }

        // Проверяем, если здоровье на нуле - персонаж умирает
        if (currentHealth == 0)
        {
            Die();
        }
    }

    // Функция для восстановления здоровья
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Вызываем событие для обновления UI
        if (onHealthChanged != null)
        {
            onHealthChanged(currentHealth, maxHealth);
        }
    }

    // Функция для смерти персонажа
    void Die()
    {
        Debug.Log("Персонаж умер!");
        gameManager.OnPlayerLoose();
        // Здесь можно добавить логику для смерти персонажа, например перезагрузка уровня
    }
}
