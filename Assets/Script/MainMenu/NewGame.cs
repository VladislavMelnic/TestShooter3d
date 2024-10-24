using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public void OpenGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();  // Сохраняем изменения

        SceneManager.LoadScene("SampleScene");
    }

    public void Continue() {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        // Для отладки в редакторе Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Останавливает игру в редакторе
#else
        Application.Quit();  // Закрывает игру в собранной версии
#endif
    }

}
