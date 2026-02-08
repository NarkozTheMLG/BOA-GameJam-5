using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float destroyTime = 1f;
    public Vector3 offset = new Vector3(0, 2f, 0); 

    void Start()
    {
        transform.localPosition += offset;

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // 3. Float Upwards
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    public void SetText(string text)
    {
        GetComponent<TMP_Text>().text = text;
    }
}