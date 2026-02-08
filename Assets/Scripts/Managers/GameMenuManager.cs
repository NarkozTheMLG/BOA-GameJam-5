using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Play butonuna basýldýðýnda
    public void PlayGame()
    {
        // Hangi sahneyi yüklemek istiyorsunuz? Scene adýný yazýn
        SceneManager.LoadScene("AnimationScene");

        // veya Build Settings'teki index ile:
        // SceneManager.LoadScene(1);
    }

    // Credits butonuna basýldýðýnda
    public void ShowCredits()
    {
        // Credits sahnesini yükle
        SceneManager.LoadScene("Credits");

        // veya Credits paneli açmak isterseniz farklý bir yöntem kullanabiliriz
    }

    // Exit butonuna basýldýðýnda
    public void ExitGame()
    {
        Debug.Log("Oyundan çýkýlýyor...");
        Application.Quit();

        // Unity Editor'de test için:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}