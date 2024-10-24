using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public void OpenGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();  // ��������� ���������

        SceneManager.LoadScene("SampleScene");
    }

    public void Continue() {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        // ��� ������� � ��������� Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // ������������� ���� � ���������
#else
        Application.Quit();  // ��������� ���� � ��������� ������
#endif
    }

}
