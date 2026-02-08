using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public bool hasIDCard = false;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();
            }
            return instance;
        }
    }

    public GameObject currentCreaturePrefab;
    public string lastDoorID; // YENÝ: Son kullanýlan kapý ID'si

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}