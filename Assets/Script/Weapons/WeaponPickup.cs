using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponToGive; // ������, ������� ��� �����
    public string playerTag = "Player"; // ��� ������, ����� ���������, ��� �������������� ���������� � ���

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������, ������� �������� ����, ����� ��� "Player"
        if (other.CompareTag(playerTag))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                // ����� ������ ������
                weaponManager.AddWeapon(weaponToGive);

                // Destroy the pickup object after it's collected
                Destroy(gameObject);

                // ���� �����, ����� ������� ��� ��� ������� ��� ���������� ����� ��������� ������
                // Destroy(gameObject); // ������� ��� ����� ��������� ������
            }
        }
    }
}
