using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float baseFollowSpeed = 10f; 
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Rigidbody2D playerRb;
    private float dynamicSpeed;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform not assigned to CameraFollow!");
            return;
        }

        playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogError("Player does not have a Rigidbody2D. Camera may not follow properly.");
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Adjust camera speed dynamically based on player movement
        float playerSpeed = playerRb != null ? playerRb.velocity.magnitude : 0f;
        dynamicSpeed = Mathf.Max(baseFollowSpeed, playerSpeed * 1.5f); // Adjust scaling as needed

        Vector3 targetPosition = player.position + offset;

        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, dynamicSpeed * Time.fixedDeltaTime);
    }
}