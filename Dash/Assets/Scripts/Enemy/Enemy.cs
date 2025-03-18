using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Base enemy health for this enemy. This value is used to calculate the final health based on the current floor.")]
    public float enemyBaseHealth = 3f;
    // The final health used in gameplay.
    private int health;

    public int soulValue = 5;
    public int damageToPlayer = 10; // Damage dealt to player on contact

    [Header("XP Settings")]
    [Tooltip("Base XP awarded by this enemy. The final XP reward is proportional to the current floor.")]
    public int baseXP = 10;
    [Tooltip("Reference to the PlayerData ScriptableObject to award XP.")]
    public PlayerDataSO playerData;

    private EnemySpawner spawner;
    private Transform player;

    public float knockbackForce = 5f;    // Strength of the knockback
    public float knockbackDuration = 0.2f; // How long knockback lasts

    public float knockbackThreshold = 5f;  // If velocity exceeds this, assume it was knocked away
    public float returnDelay = 3f;         // Time before starting to return
    public float returnSpeed = 2f;         // Speed at which it returns

    private Rigidbody2D rb;
    private bool returningToBattle = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Enemy needs a Rigidbody2D component!");
        }

        // If no spawner has set the enemy's final health, default to the base value.
        if (spawner == null)
        {
            health = Mathf.RoundToInt(enemyBaseHealth);
        }
    }

    private void Update()
    {
        if (!returningToBattle && rb.velocity.magnitude > knockbackThreshold)
        {
            StartCoroutine(ReturnToBattle());
        }
    }

    private IEnumerator ReturnToBattle()
    {
        returningToBattle = true;
        yield return new WaitForSeconds(returnDelay); // Wait before returning

        while (Vector2.Distance(transform.position, player.position) > 2f) // Return until close
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * returnSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero; // Stop moving once back
        returningToBattle = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Award souls via your SoulManager.
        SoulManager.Instance.AddSouls(soulValue);

        // Award XP to the player.
        if (playerData != null)
        {
            // Calculate XP reward proportional to the player's current floor.
            int xpReward = baseXP * playerData.currentFloor;
            playerData.currentExp += xpReward;
            Debug.Log("Enemy died: Awarded XP: " + xpReward);
        }

        // Inform the spawner that this enemy is dead.
        if (spawner != null)
        {
            spawner.EnemyDied();
        }

        Destroy(gameObject);
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    /// <summary>
    /// Calculates and sets the enemy's final health using its base health.
    /// Since Health and base enemy health are the same, the final health is:
    /// enemyBaseHealth + (floor * enemyHealthIncreasePerFloor)
    /// </summary>
    /// <param name="floor">The current floor level.</param>
    /// <param name="enemyHealthIncreasePerFloor">The additional enemy health per floor (from the spawner).</param>
    public void SetFinalHealth(int floor, float enemyHealthIncreasePerFloor)
    {
        health = Mathf.RoundToInt(enemyBaseHealth + (floor * enemyHealthIncreasePerFloor));
    }

    // Damage and knockback player on collision.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }

            if (playerRb != null)
            {
                StartCoroutine(ApplyKnockback(playerRb, collision.transform.position));
            }
        }
    }

    private IEnumerator ApplyKnockback(Rigidbody2D playerRb, Vector2 playerPosition)
    {
        Vector2 knockbackDirection = (playerPosition - (Vector2)transform.position).normalized;
        playerRb.velocity = knockbackDirection * knockbackForce;

        yield return new WaitForSeconds(knockbackDuration);
        playerRb.velocity = Vector2.zero; // Stop knockback effect after duration
    }
}
