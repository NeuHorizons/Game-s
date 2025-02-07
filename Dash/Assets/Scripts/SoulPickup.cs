using UnityEngine;

public class SoulPickup : MonoBehaviour
{
    public int soulValue = 5; // How many souls this pickup gives

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.CollectSoul(soulValue);
            Destroy(gameObject); // Remove soul pickup from scene
        }
    }
}