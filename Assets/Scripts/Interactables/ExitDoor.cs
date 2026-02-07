using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private string mainHallSceneName = "MainHall";

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Çýkýþ kapýsýna yaklaþtýnýz! E tuþuna basýn.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ReturnToMainHall();
        }
    }

    private void ReturnToMainHall()
    {
        SceneManager.LoadScene(mainHallSceneName);
    }
}