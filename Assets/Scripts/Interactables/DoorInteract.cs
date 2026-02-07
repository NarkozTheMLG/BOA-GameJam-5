using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private string roomSceneName = "Room";
    [SerializeField] private string doorID; // YENÝ: Her kapýnýn benzersiz ID'si

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Kapýya yaklaþtýnýz! E tuþuna basýn.");
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
            EnterRoom();
        }
    }

    private void EnterRoom()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentCreaturePrefab = creaturePrefab;
            GameManager.Instance.lastDoorID = doorID; // YENÝ: Hangi kapýdan girdiðimizi kaydet
            SceneManager.LoadScene(roomSceneName);
        }
    }

    public string GetDoorID()
    {
        return doorID;
    }
}