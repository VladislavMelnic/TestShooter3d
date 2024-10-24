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

        // ������������ �������� ���������
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // �������� �������, ����� �������� ����������� HP �� ������
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        // ���������, ���� �������� �� ���� - �������� �������
        if (currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy killed");
        GameManager.Instance.EnemyKilled();  // ���������� Singleton ��� ������� � GameManager
        Destroy(gameObject);  // ���������� ������ �����
    }
}
