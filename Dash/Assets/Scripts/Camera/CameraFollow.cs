using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign Player object in Unity Inspector
    public float smoothSpeed = 5f; // Adjust for smoother motion
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Keeps camera behind

    public bool useBounds = false;
    public Vector2 minBounds; // Set these values if using bounds
    public Vector2 maxBounds;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + offset;

        // Apply bounds if enabled
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        }

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}