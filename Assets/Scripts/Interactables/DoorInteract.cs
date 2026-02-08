using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteract : MonoBehaviour
{
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private string roomSceneName = "Room";
    [SerializeField] private string doorID;

    [Header("ID Card Requirement")]
    [SerializeField] private bool requiresIDCard = false;

    [Header("UI (Optional)")]
    [SerializeField] private GameObject lockedMessageUI;
    [SerializeField] private float messageDisplayTime = 2f; // Mesaj kaç saniye gösterilsin

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
            HideLockedMessage(); // Kapýdan uzaklaþýnca mesajý gizle
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            EnterRoom();
        }
    }

    private bool HasIDCard()
    {
        return GameManager.Instance != null && GameManager.Instance.hasIDCard;
    }

    private void EnterRoom()
    {
        // ID kartý kontrolü - E tuþuna basýldýðýnda
        if (requiresIDCard && !HasIDCard())
        {
            Debug.Log("Kapý kilitli! Önce ID kartýný bulmalýsýnýz.");
            ShowLockedMessage();
            return;
        }

        // Kapýyý aç
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentCreaturePrefab = creaturePrefab;
            GameManager.Instance.lastDoorID = doorID;
            SceneManager.LoadScene(roomSceneName);
        }
    }

    private void ShowLockedMessage()
    {
        if (lockedMessageUI != null)
        {
            lockedMessageUI.SetActive(true);
            // Belirli süre sonra otomatik gizle
            Invoke("HideLockedMessage", messageDisplayTime);
        }
    }

    private void HideLockedMessage()
    {
        if (lockedMessageUI != null)
        {
            lockedMessageUI.SetActive(false);
        }
    }

    public string GetDoorID()
    {
        return doorID;
    }
}