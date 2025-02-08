using UnityEngine;

public class BeaconHealth : MonoBehaviour
{
    public int health = 5;

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
        Debug.Log("Spawner destroyed!");
        Destroy(gameObject);
    }
}