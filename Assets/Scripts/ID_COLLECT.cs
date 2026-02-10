using UnityEngine;

public class IDCard : MonoBehaviour
{
    [Header("Float Effect")]
    public float floatSpeed = 1f;       
    public float floatAmplitude = 0.3f;  

    [Header("Rotation (Optional)")]
    public bool rotateCard = true;
    public float rotateSpeed = 50f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

      
      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectID();
        }
    }

    void CollectID()
    {
        Debug.Log("ID Kart� topland�!");


        Destroy(gameObject);
    }
}