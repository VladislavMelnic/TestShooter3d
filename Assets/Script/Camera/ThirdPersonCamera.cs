using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; // ������ �� ������ ������
    public float distance = 5f; // ���������� �� ������
    public float height = 2f; // ������ ������
    public float damping = 3f; // ��������� ��� �������� ����������

    private void LateUpdate()
    {
        if (player == null) return;

        // ���������� ������� ������
        Vector3 desiredPosition = player.position - player.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);

        // ������� �� ������
        transform.LookAt(player);
    }
}
