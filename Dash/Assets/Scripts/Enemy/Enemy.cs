using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public int soulValue = 5;
    private EnemySpawner spawner;
    private Transform player;

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
        FindObjectOfType<PlayerMovement>().CollectSoul(soulValue);
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
}