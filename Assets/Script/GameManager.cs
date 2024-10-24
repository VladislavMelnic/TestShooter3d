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
            Instance = this;  // ��������� ������� ���������
        }
        else
        {
            Destroy(gameObject);  // ����������, ���� ��� ���� ���������
        }
    }

    public GameObject loosePopup; // ������ �� ������ loosePopup-������
    public GameObject winPopup;  // ������ �� �������� �����

    public int enemiesToKill = 4;  // ���������� ������ ��� ������
    private int enemiesKilled = 0;  // ������� ������ ������

    // ����� ���������� �������
    private int totalKills;

    public TMP_Text killsText; // ������ �� UI TextMeshPro ���������


    public TMP_Text winText;       // ������ �� TextMeshPro ��� �����
    public TMP_Text looseText;     // ������ �� TextMeshPro ��� ���������

    private int wins;              // ���������� �����
    private int losses;            // ���������� ���������

    private void Start()
    {
        // ��������� ����� ���������� ������� �� PlayerPrefs
        totalKills = PlayerPrefs.GetInt("TotalKills", 0);
        wins = PlayerPrefs.GetInt("Wins", 0);
        losses = PlayerPrefs.GetInt("Losses", 0);
        UpdateText();
    }

    public void OnPlayerLoose()
    {
        Time.timeScale = 0;  // ������������� �����
        losses++;  // ����������� ���������� ���������
        PlayerPrefs.SetInt("Losses", losses);  // ��������� ���������� ���������
        PlayerPrefs.Save();  // ��������� ���������

        loosePopup.SetActive(true);  // ���������� ����� ���������
        looseText.text = $"Losses: {losses}";  // ��������� ����� ��������� �� ������
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClosePopupLoose()
    {
        Time.timeScale = 1; // ���������� �����
        loosePopup.SetActive(false); // ��������� ����
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        totalKills++; // ����������� ����� ���������� �������
        PlayerPrefs.SetInt("TotalKills", totalKills); // ��������� ����� ���������� �������
        PlayerPrefs.Save(); // ��������� ���������
        Debug.Log("Enemies killed: " + enemiesKilled);
        Debug.Log("Total kills: " + totalKills);

        UpdateText(); // ��������� ����� �� UI

        // ���� ����� ����������� ���������� ������, �������� ������
        if (enemiesKilled >= enemiesToKill)
        {
            Win();
        }
    }

    private void Win()
    {
        Time.timeScale = 0;  // ������������� �����
        wins++;  // ����������� ���������� �����
        PlayerPrefs.SetInt("Wins", wins);  // ��������� ���������� �����
        PlayerPrefs.Save();  // ��������� ���������

        winPopup.SetActive(true);  // ���������� �������� �����
        winText.text = $"Wins: {wins}";  // ��������� ����� ����� �� ������
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnClosePopupWin()
    {
        Time.timeScale = 1;  // ���������� �����
        winPopup.SetActive(false);  // ��������� �����
       // Cursor.visible = false;
       // Cursor.lockState = CursorLockMode.Locked;
    }

  

    private void UpdateText()
    {
        killsText.text = $"Total kills: {totalKills}"; // ���������� ����� ���������� �������
    }


    public void onTryAgain()
    {
       
        OnClosePopupLoose();
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // ������������� ������� �����
        SceneManager.LoadScene(1);  // ������������� ������� �����
    }

    public void onMainMenu()
    {
        Time.timeScale = 1;
        OnClosePopupWin();
        SceneManager.LoadScene(0);
    }
}
