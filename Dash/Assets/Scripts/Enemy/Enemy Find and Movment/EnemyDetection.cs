using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDetection : MonoBehaviour
{
    public float visionRange = 8f;
    public float visionAngle = 90f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public MonoBehaviour movementScript; // ✅ Drag movement script here
    public MonoBehaviour aiPathScript; // ✅ Drag A* Pathfinding script here

    private Transform player;
    private bool hasSeenPlayer = false;
    public static bool hiveMindAlert = false; // ✅ Stays true once called, stops raycasting

    public static List<EnemyDetection> allEnemies = new List<EnemyDetection>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        allEnemies.Add(this);

        if (movementScript != null)
            movementScript.enabled = false; // ✅ Disable movement initially

        if (aiPathScript != null)
            aiPathScript.enabled = false; // ✅ Disable A* initially

        // ✅ If hive mind is already active when this enemy spawns, start chasing
        if (hiveMindAlert)
        {
            ActivateChase();
        }
    }

    private void Update()
    {
        if (hiveMindAlert) return; // ✅ Stop raycasting once hive mind is activated

        if (CanSeePlayer())
        {
            AlertAllEnemies();
            ActivateChase();
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

    public static void AlertAllEnemies()
    {
        hiveMindAlert = true; // ✅ Stops raycasting permanently until reset
        foreach (EnemyDetection enemy in allEnemies)
        {
            enemy.hasSeenPlayer = true;
            enemy.ActivateChase();
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

    public void ActivateChase()
    {
        if (movementScript != null)
            movementScript.enabled = true; // ✅ Enables movement

        if (aiPathScript != null)
            aiPathScript.enabled = false; // ✅ Ensure pathfinding is off initially
    }

    public void DeactivateChase()
    {
        if (movementScript != null)
            movementScript.enabled = false; // ✅ Disable movement when level resets

        if (aiPathScript != null)
            aiPathScript.enabled = false; // ✅ Ensure pathfinding stops
    }

    private void OnDestroy()
    {
        allEnemies.Remove(this);
    }
}
