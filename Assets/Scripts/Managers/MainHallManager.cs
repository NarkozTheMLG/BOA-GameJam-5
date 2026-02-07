using UnityEngine;

public class MainHallManager : MonoBehaviour
{
    void Start()
    {
        PositionPlayerAtLastDoor();
    }

    private void PositionPlayerAtLastDoor()
    {
        if (GameManager.Instance == null || string.IsNullOrEmpty(GameManager.Instance.lastDoorID))
        {
            return; // Ýlk baþlangýç, bir þey yapma
        }

        // Son kullanýlan kapýyý bul
        DoorInteract[] doors = FindObjectsByType<DoorInteract>(FindObjectsSortMode.None);

        foreach (DoorInteract door in doors)
        {
            if (door.GetDoorID() == GameManager.Instance.lastDoorID)
            {
                // Player'ý bu kapýnýn yanýna yerleþtir
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 spawnPos = door.transform.position + Vector3.left * 2f;
                    spawnPos.y = -1.8f; // Zemin seviyesi (Ground'unuzun y pozisyonuna göre ayarlayýn)
                    player.transform.position = spawnPos;
                }
                break;
            }
        }
    }
}