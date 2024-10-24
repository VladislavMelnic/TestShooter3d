using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // Singleton

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  // Назначаем текущий экземпляр
        }
        else
        {
            Destroy(gameObject);  // Уничтожаем, если уже есть экземпляр
        }
    }

    public GameObject loosePopup; // Ссылка на объект loosePopup-попапа
    public GameObject winPopup;  // Ссылка на победный попап

    public int enemiesToKill = 4;  // Количество врагов для победы
    private int enemiesKilled = 0;  // Счетчик убитых врагов

    // Общее количество убийств
    private int totalKills;

    public TMP_Text killsText; // Ссылка на UI TextMeshPro компонент


    public TMP_Text winText;       // Ссылка на TextMeshPro для побед
    public TMP_Text looseText;     // Ссылка на TextMeshPro для поражений

    private int wins;              // Количество побед
    private int losses;            // Количество поражений

    private void Start()
    {
        // Загружаем общее количество убийств из PlayerPrefs
        totalKills = PlayerPrefs.GetInt("TotalKills", 0);
        wins = PlayerPrefs.GetInt("Wins", 0);
        losses = PlayerPrefs.GetInt("Losses", 0);
        UpdateText();
    }

    public void OnPlayerLoose()
    {
        Time.timeScale = 0;  // Останавливаем время
        losses++;  // Увеличиваем количество поражений
        PlayerPrefs.SetInt("Losses", losses);  // Сохраняем количество поражений
        PlayerPrefs.Save();  // Сохраняем изменения

        loosePopup.SetActive(true);  // Показываем попап поражения
        looseText.text = $"Losses: {losses}";  // Обновляем текст поражений на попапе
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClosePopupLoose()
    {
        Time.timeScale = 1; // Возвращаем время
        loosePopup.SetActive(false); // Закрываем окно
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        totalKills++; // Увеличиваем общее количество убийств
        PlayerPrefs.SetInt("TotalKills", totalKills); // Сохраняем общее количество убийств
        PlayerPrefs.Save(); // Сохраняем изменения
        Debug.Log("Enemies killed: " + enemiesKilled);
        Debug.Log("Total kills: " + totalKills);

        UpdateText(); // Обновляем текст на UI

        // Если убито достаточное количество врагов, вызываем победу
        if (enemiesKilled >= enemiesToKill)
        {
            Win();
        }
    }

    private void Win()
    {
        Time.timeScale = 0;  // Останавливаем время
        wins++;  // Увеличиваем количество побед
        PlayerPrefs.SetInt("Wins", wins);  // Сохраняем количество побед
        PlayerPrefs.Save();  // Сохраняем изменения

        winPopup.SetActive(true);  // Показываем победный попап
        winText.text = $"Wins: {wins}";  // Обновляем текст побед на попапе
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClosePopupWin()
    {
        Time.timeScale = 1;  // Возвращаем время
        winPopup.SetActive(false);  // Закрываем попап
       // Cursor.visible = false;
       // Cursor.lockState = CursorLockMode.Locked;
    }

  

    private void UpdateText()
    {
        killsText.text = $"Total kills: {totalKills}"; // Отображаем общее количество убийств
    }


    public void onTryAgain()
    {
       
        OnClosePopupLoose();
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Перезагружаем текущую сцену
        SceneManager.LoadScene(1);  // Перезагружаем текущую сцену
    }

    public void onMainMenu()
    {
        Time.timeScale = 1;
        OnClosePopupWin();
        SceneManager.LoadScene(0);
    }
}
