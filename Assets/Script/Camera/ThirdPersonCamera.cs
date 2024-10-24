using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; // Ссылка на объект игрока
    public float distance = 5f; // Расстояние до игрока
    public float height = 2f; // Высота камеры
    public float damping = 3f; // Затухание для плавного следования

    private void LateUpdate()
    {
        if (player == null) return;

        // Определяем позицию камеры
        Vector3 desiredPosition = player.position - player.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);

        // Смотрим на игрока
        transform.LookAt(player);
    }
}
