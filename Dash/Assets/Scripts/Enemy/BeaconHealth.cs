using UnityEngine;

public class BeaconHealth : MonoBehaviour
{
    public int health = 3;
    private EnemySpawner spawner;

    private void Start()
    {
        spawner = GetComponent<EnemySpawner>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyBeacon();
        }
    }

    void DestroyBeacon()
    {
        if (spawner != null)
        {
            spawner.DestroySpawner();
        }
        Destroy(gameObject);
    }
}