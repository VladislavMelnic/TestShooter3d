//using UnityEngine;
//using UnityEngine.UI;

//public class HealthUI : MonoBehaviour
//{
//    public PlayerHealth playerHealth; // ������ �� ������ � HP
//    public Slider healthSlider;       // ������ �� ������� UI

//    void Start()
//    {
//        // ������������� �� ������� ��������� ��������
//        playerHealth.onHealthChanged += UpdateHealthBar;

//        // �������������� ������� ���������� ��������
//        healthSlider.maxValue = playerHealth.maxHealth;
//        healthSlider.value = playerHealth.currentHealth;
//    }

//    // ��������� �������� ��������
//    void UpdateHealthBar(int currentHealth, int maxHealth)
//    {
//        healthSlider.value = currentHealth;
//    }
//}
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // ������ �� �������� ��������
    public PlayerHealth playerHealth; // ������ �� ��������� PlayerHealth

    void Start()
    {
        if (playerHealth != null)
        {
            // ������������� �� ������� ��������� ��������
            playerHealth.onHealthChanged += UpdateHealthBar;
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
        if (playerHealth != null)
        {
            playerHealth.onHealthChanged -= UpdateHealthBar;
        }
    }
}
