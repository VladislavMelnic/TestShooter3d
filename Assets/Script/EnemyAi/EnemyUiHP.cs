using UnityEngine;
using UnityEngine.UI;

public class EnemyUiHP : MonoBehaviour

{ 
public Slider healthSlider; // ������ �� �������� ��������
public EnemyHP enemyHealth; // ������ �� ��������� PlayerHealth

void Start()
{
    if (enemyHealth != null)
    {
            // ������������� �� ������� ��������� ��������
            enemyHealth.onHealthChanged += UpdateHealthBar;
    }
}

// ����� ��� ���������� �������� �������� ��������
void UpdateHealthBar(int currentHealth, int maxHealth)
{
    healthSlider.value = (float)currentHealth / maxHealth; // ������������� �������� ��������
    Debug.Log("Health bar updated: " + healthSlider.value);
}

void OnDestroy()
{
    // ������������ �� �������, ����� �������� ������
    if (enemyHealth != null)
    {
            enemyHealth.onHealthChanged -= UpdateHealthBar;
    }
}
}