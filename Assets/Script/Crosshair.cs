using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public GameObject crosshair; // Ссылка на объект прицела

    private void Start()
    {
        crosshair.SetActive(true); // Включаем прицел при старте
    }

    public void ShowCrosshair()
    {
        crosshair.SetActive(true); // Показать прицел
    }

    public void HideCrosshair()
    {
        crosshair.SetActive(false); // Скрыть прицел
    }
}
