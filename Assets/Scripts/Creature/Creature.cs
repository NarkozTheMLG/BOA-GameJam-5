using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private string creatureName = "Zombie";

    void Start()
    {
        Debug.Log(creatureName + " odaya spawn oldu!");
    }
}