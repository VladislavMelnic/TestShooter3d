using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // ������������ ��������
    public int currentHealth;   // ������� ��������


    public GameManager gameManager; // ������ �� GameManager

    // ������� ������, ����� �������� �������� ����
    public delegate void OnHealthChanged(int currentHealth, int maxHealth);
    public event OnHealthChanged onHealthChanged;

    void Start()
    {
        currentHealth = maxHealth; // ������������� ��������� ��������
    }

    // ������� ��� ��������� ����� ���������
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // ������������ �������� ���������
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // �������� �������, ����� �������� ����������� HP �� ������
        if (onHealthChanged != null)
        {

            Debug.Log(currentHealth);
            Debug.Log(maxHealth);
            onHealthChanged(currentHealth, maxHealth);
        }

        // ���������, ���� �������� �� ���� - �������� �������
        if (currentHealth == 0)
        {
            Die();
        }
    }

    // ������� ��� �������������� ��������
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // �������� ������� ��� ���������� UI
        if (onHealthChanged != null)
        {
            onHealthChanged(currentHealth, maxHealth);
        }
    }

    // ������� ��� ������ ���������
    void Die()
    {
        Debug.Log("�������� ����!");
        gameManager.OnPlayerLoose();
        // ����� ����� �������� ������ ��� ������ ���������, �������� ������������ ������
    }
}
