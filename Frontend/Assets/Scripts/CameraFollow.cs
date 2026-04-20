using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    [Header("Smooth Time")]
    public float smoothTimeX = 0.08f;
    public float smoothTimeY = 0.15f;

    [Header("Vertical Dead Zone")]
    public bool followY = true;
    public float yDeadZone = 1.5f;

    [Header("Camera Bounds")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private float velocityX = 0f;
    private float velocityY = 0f;
    private float currentY;

    private Camera cam;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();

        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;

        if (target != null)
        {
            currentY = target.position.y + offset.y;

            float startX = Mathf.Clamp(target.position.x + offset.x, minX + halfWidth, maxX - halfWidth);
            float startY = Mathf.Clamp(currentY, minY + halfHeight, maxY - halfHeight);

            transform.position = new Vector3(startX, startY, offset.z);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        float targetX = target.position.x + offset.x;
        float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocityX, smoothTimeX);

        float newY = transform.position.y;

        if (followY)
        {
            float desiredY = target.position.y + offset.y;

            if (Mathf.Abs(desiredY - currentY) > yDeadZone)
            {
                currentY = desiredY;
            }

            newY = Mathf.SmoothDamp(transform.position.y, currentY, ref velocityY, smoothTimeY);
        }

        // Clamp camera center so camera edges stay inside world bounds
        newX = Mathf.Clamp(newX, minX + halfWidth, maxX - halfWidth);
        newY = Mathf.Clamp(newY, minY + halfHeight, maxY - halfHeight);

        transform.position = new Vector3(newX, newY, offset.z);
    }
}