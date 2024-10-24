using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public delegate void OnHealthChanged(int currentHealth, int maxHealth);
    public event OnHealthChanged onHealthChanged;

    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Ограничиваем здоровье минимумом
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Вызываем событие, чтобы обновить отображение HP на экране
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        // Проверяем, если здоровье на нуле - персонаж умирает
        if (currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy killed");
        GameManager.Instance.EnemyKilled();  // Используем Singleton для доступа к GameManager
        Destroy(gameObject);  // Уничтожаем объект врага
    }
}
