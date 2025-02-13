using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public int soulValue = 5;
    public int damageToPlayer = 10; // Damage dealt to player on contact

    private EnemySpawner spawner;
    private Transform player;

    public float knockbackForce = 5f; // Strength of the knockback
    public float knockbackDuration = 0.2f; // How long knockback lasts

    public float knockbackThreshold = 5f; // If velocity exceeds this, assume it was knocked away
    public float returnDelay = 3f; // Time before starting to return
    public float returnSpeed = 2f; // Speed at which it returns

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
        SoulManager.Instance.AddSouls(soulValue);

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

    // Damage and Knockback Player on Collision
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
