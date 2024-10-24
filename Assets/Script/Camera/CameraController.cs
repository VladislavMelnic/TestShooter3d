using UnityEngine;
using UnityEngine.InputSystem; // �� �������� �������� ���� using

public class CameraController : MonoBehaviour
{
    public Transform player; // ������ �� ������ ������
    public float mouseSensitivity = 100f; // ���������������� ����
    public float verticalRotationLimit = 80f; // ����������� ������������� ��������

    private float xRotation = 0f; // ������ ������� ������������ ��������

    public Vector3 offset; // �������� ������ ������������ ������
    public float smoothSpeed = 0.125f; // �������� �����������



    private void Start()
    {
        // �������� ������
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // �������� �������� ���� �� ������ Input System
        var mousePosition = Mouse.current.delta.ReadValue();
        float mouseX = mousePosition.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mousePosition.y * mouseSensitivity * Time.deltaTime;

        // ������������ ��������
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit); // ������������ ������������ ��������

        // ��������� �������� � CameraHolder
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // �������� ������� ������ �� �����������
        player.Rotate(Vector3.up * mouseX);
    }


}

