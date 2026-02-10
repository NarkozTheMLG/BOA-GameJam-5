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
            return; 
        }

        DoorInteract[] doors = FindObjectsByType<DoorInteract>(FindObjectsSortMode.None);

        foreach (DoorInteract door in doors)
        {
            if (door.GetDoorID() == GameManager.Instance.lastDoorID)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 spawnPos = door.transform.position + Vector3.left * 2f;
                    spawnPos.y = -1.8f; 
                    player.transform.position = spawnPos;
                }
                break;
            }
        }
    }
}