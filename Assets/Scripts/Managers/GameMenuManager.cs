using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("AnimationScene");
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        Debug.Log("Oyundan ��k�l�yor...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}