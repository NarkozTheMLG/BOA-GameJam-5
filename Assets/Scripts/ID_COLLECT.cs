using UnityEngine;

public class IDCard : MonoBehaviour
{
    [Header("Float Effect")]
    public float floatSpeed = 1f;        // Hareket hýzý
    public float floatAmplitude = 0.3f;  // Yukarý-aþaðý mesafesi

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
        // Yukarý-aþaðý hareket (sine wave kullanarak)
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
        Debug.Log("ID Kartý toplandý!");

        // Burada ses efekti ekleyebilirsiniz
        // AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // Inventory'ye ekle
        GameManager.Instance.hasIDCard = true;

        Destroy(gameObject);
    }
}