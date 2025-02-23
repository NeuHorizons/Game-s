using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDetection : MonoBehaviour
{
    public float visionRange = 8f;
    public float visionAngle = 90f;
    public float alertDuration = 3f;
    public float fleeChance = 0.3f;
    public float fleeSpeed = 4f;
    public float fleeTime = 2f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public MonoBehaviour movementScript; // Drag the movement script in the Inspector

    private Transform player;
    private bool hasSeenPlayer = false;
    private static bool hiveMindAlert = false; // Keeps track if all enemies are alert
    private float alertTimer = 0f;
    private bool isFleeing = false;

    public static List<EnemyDetection> allEnemies = new List<EnemyDetection>();

    private Rigidbody2D rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        allEnemies.Add(this);

        if (movementScript != null)
        {
            movementScript.enabled = false; // Disable movement initially
        }

        // ✅ If hive mind is already active when this enemy spawns, make it chase immediately
        if (hiveMindAlert)
        {
            ActivateChase();
        }
    }

    private void Update()
    {
        if (hiveMindAlert) return; // Stop checking vision once hive mind is active

        if (CanSeePlayer())
        {
            if (!hasSeenPlayer)
            {
                hasSeenPlayer = true;
                alertTimer = alertDuration;

                if (Random.value < fleeChance)
                {
                    StartCoroutine(FleeAndAlert());
                }
                else
                {
                    AlertAllEnemies();
                    ActivateChase();
                }
            }
        }

        if (hasSeenPlayer)
        {
            alertTimer -= Time.deltaTime;
            if (alertTimer <= 0)
            {
                AlertAllEnemies();
            }
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, visionRange, playerLayer);
        if (playerCollider == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.right, directionToPlayer);
        if (angle > visionAngle / 2f) return false;

        if (Physics2D.Raycast(transform.position, directionToPlayer, visionRange, obstacleLayer))
        {
            return false; // Blocked by an obstacle
        }

        return true;
    }

    private IEnumerator FleeAndAlert()
    {
        isFleeing = true;
        Vector2 fleeDirection = (transform.position - player.position).normalized;

        float fleeTimer = fleeTime;
        while (fleeTimer > 0)
        {
            rb.velocity = fleeDirection * fleeSpeed; // Moves enemy away
            fleeTimer -= Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isFleeing = false;
        AlertAllEnemies();
    }

    public static void AlertAllEnemies()
    {
        hiveMindAlert = true; // ✅ Ensures all future spawns inherit the alert state
        foreach (EnemyDetection enemy in allEnemies)
        {
            enemy.hasSeenPlayer = true;
            enemy.ActivateChase();
        }
    }

    public void ActivateChase()
    {
        if (movementScript != null)
        {
            movementScript.enabled = true; // Enables the assigned movement script
        }
    }

    public static void ResetHiveMind()
    {
        hiveMindAlert = false;
        foreach (EnemyDetection enemy in allEnemies)
        {
            enemy.hasSeenPlayer = false;
            enemy.DeactivateChase();
        }
    }

    public void DeactivateChase()
    {
        if (movementScript != null)
        {
            movementScript.enabled = false; // Disables movement when level resets
        }
    }

    private void OnDestroy()
    {
        allEnemies.Remove(this);
    }
}
