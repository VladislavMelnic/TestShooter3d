using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public GameObject crosshair; // ������ �� ������ �������

    private void Start()
    {
        crosshair.SetActive(true); // �������� ������ ��� ������
    }

    public void ShowCrosshair()
    {
        crosshair.SetActive(true); // �������� ������
    }

    public void HideCrosshair()
    {
        crosshair.SetActive(false); // ������ ������
    }
}
