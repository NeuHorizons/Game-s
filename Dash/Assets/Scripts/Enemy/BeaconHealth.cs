using UnityEngine;

public class BeaconHealth : MonoBehaviour
{
    public int health = 5;
    
    // Assign your drop prefab in the Inspector.
    public GameObject dropPrefab;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Spawner took damage! Remaining health: " + health);

        if (health <= 0)
        {
            DestroySpawner();
        }
    }

    void DestroySpawner()
    {
        Debug.Log("Spawner destroyed! Spawning drop...");
        SpawnDrop();
        Destroy(gameObject);
    }

    void SpawnDrop()
    {
        if (dropPrefab != null)
        {
            Debug.Log("Spawning drop: " + dropPrefab.name + " at position: " + transform.position);
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("dropPrefab is not assigned! No drop will spawn.");
        }
    }
}