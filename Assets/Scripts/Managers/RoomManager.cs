using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Transform creatureSpawnPoint;

    void Start()
    {
        SpawnCreature();
    }

    private void SpawnCreature()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentCreaturePrefab != null)
        {
            Instantiate(GameManager.Instance.currentCreaturePrefab,
                       creatureSpawnPoint.position,
                       Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Yaratýk prefab'ý bulunamadý!");
        }
    }
}