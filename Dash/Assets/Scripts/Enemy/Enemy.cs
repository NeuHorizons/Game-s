using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public int soulValue = 5; // Souls dropped when killed
    private Transform player;
    private EnemySpawner spawner; // Reference to the spawner

    // Chance-based actions
    [Range(0f, 1f)] public float teleportChance = 0.2f; // 20% chance to teleport
    [Range(0f, 1f)] public float counterAttackChance = 0.3f; // 30% chance to counterattack

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        // Chance to teleport away instead of taking damage
        if (Random.value < teleportChance)
        {
            TeleportAway();
            return;
        }

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void TeleportAway()
    {
        Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * 3f;
        transform.position = randomPosition;
    }

    void Die()
    {
        // Grant souls to the player
        FindObjectOfType<PlayerMovement>().CollectSoul(soulValue);

        // Notify the spawner (if exists)
        if (spawner != null)
        {
            spawner.EnemyDied();
        }

        Destroy(gameObject);
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner; // Assign the spawner reference
    }
}