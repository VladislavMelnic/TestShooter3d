using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;           // ��� ������
    public int damage;                  // ���� �� ������
    public int bulletsPerMagazine;      // ���������� �������� � ������
    public int totalBullets;            // ����� ���������� ��������
    public GameObject weaponPrefab;     // ������ ��� ����� ������
    public bool isAutomatic; // �������������� �����
    public float fireRate; // �������� ����� ���������� (��� ��������������� ������)
    public Sprite imageSprite;

    public ParticleSystem muzzleFlash; // ��� ��� ParticleSystem ��� ��������

    [HideInInspector]
    public int currentBullets;          // ������� ���������� �������� � ������

    private void OnEnable()
    {
        // ������������� �������� ���������� ��������
        currentBullets = bulletsPerMagazine;
    }

    // ����� ��� ��������
    public bool Shoot()
    {
        if (currentBullets > 0)
        {
            // ������ �������� (��������, ������� ������ ��� Raycast)
            currentBullets--; // ��������� ���������� ��������
            // ������ ������� ��������
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
            return true; // ���������� true, ���� ������� ��������
        }
        else
        {
            Debug.Log("��� �������� � ��������!");
            return false; // ��� �������� ��� ��������
        }
    }

    // ����� ��� �����������
    public void Reload()
    {
        if (totalBullets > 0 && currentBullets < bulletsPerMagazine)
        {
            int bulletsNeeded = bulletsPerMagazine - currentBullets; // ������� �������� ����� ��� ������ ������
            int bulletsToReload = Mathf.Min(bulletsNeeded, totalBullets); // �� ������, ��� ���� � ������

            currentBullets += bulletsToReload; // ��������� ������� ���������� ��������
            totalBullets -= bulletsToReload;    // ��������� ����� ���������� ��������
            Debug.Log("�����������: ��������� " + bulletsToReload + " ��������. ��������: " + totalBullets);
        }
        else
        {
            Debug.Log("��� �������� ��� ����������� ��� ������� ������!");
        }
    }
}
