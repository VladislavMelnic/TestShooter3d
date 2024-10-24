using UnityEngine;
using UnityEngine.InputSystem; // Не забудьте добавить этот using

public class CameraController : MonoBehaviour
{
    public Transform player; // Ссылка на объект игрока
    public float mouseSensitivity = 100f; // Чувствительность мыши
    public float verticalRotationLimit = 80f; // Ограничение вертикального вращения

    private float xRotation = 0f; // Хранит текущее вертикальное вращение

    public Vector3 offset; // Смещение камеры относительно игрока
    public float smoothSpeed = 0.125f; // Скорость сглаживания



    private void Start()
    {
        // Скрываем курсор
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Получаем движение мыши из нового Input System
        var mousePosition = Mouse.current.delta.ReadValue();
        float mouseX = mousePosition.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mousePosition.y * mouseSensitivity * Time.deltaTime;

        // Вертикальное вращение
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit); // Ограничиваем вертикальное вращение

        // Применяем вращение к CameraHolder
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Вращение объекта игрока по горизонтали
        player.Rotate(Vector3.up * mouseX);
    }


}

