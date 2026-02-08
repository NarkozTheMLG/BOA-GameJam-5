using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    [Header("Camera Bounds")]
    public float minX;
    public float maxX;

    private float fixedY;

    void Start()
    {
        // Kameranın başlangıç Y'sini kilitle
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (!target) return;

        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float clampedX = Mathf.Clamp(
            target.position.x,
            minX + camWidth,
            maxX - camWidth
        );

        Vector3 desiredPosition = new Vector3(
            clampedX,
            fixedY,                 // 🔒 Y SABİT
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothing * Time.deltaTime
        );
    }
}
