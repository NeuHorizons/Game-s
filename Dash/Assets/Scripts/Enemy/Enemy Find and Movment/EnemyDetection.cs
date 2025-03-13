using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDetection : MonoBehaviour
{
    // Set this to the distance at which the enemy becomes active.
    public float activationDistance = 8f;
    public LayerMask playerLayer;

    public MonoBehaviour movementScript; // Drag movement script here
    public MonoBehaviour aiPathScript;   // Drag A* Pathfinding script here

    private Transform player;
    public static bool hiveMindAlert = false; // Once activated, all enemies start chasing
    public static List<EnemyDetection> allEnemies = new List<EnemyDetection>();

    private void Awake()
    {
        // Locate the player once; useful for pooled objects.
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Player not found. Make sure the player has the tag 'Player'.");
    }

    private void OnEnable()
    {
        // Add this enemy to the hive mind list if it's not already there.
        if (!allEnemies.Contains(this))
            allEnemies.Add(this);

        // Disable movement/pathfinding until the player is in range.
        if (movementScript != null)
            movementScript.enabled = false;
        if (aiPathScript != null)
            aiPathScript.enabled = false;

        // If the hive mind is already activated, start chasing immediately.
        if (hiveMindAlert)
        {
            ActivateChase();
        }
        else
        {
            // Use a coroutine to delay the initial detection check for one frame.
            StartCoroutine(CheckDetectionOnEnable());
        }
    }

    private IEnumerator CheckDetectionOnEnable()
    {
        yield return new WaitForEndOfFrame();

        if (!hiveMindAlert && IsPlayerInRange())
        {
            Debug.Log(gameObject.name + " detects player on enable.");
            AlertAllEnemies();
            ActivateChase();
        }
    }

    private void Update()
    {
        // If the hive mind is active, skip the distance check.
        if (hiveMindAlert)
            return;

        if (IsPlayerInRange())
        {
            Debug.Log(gameObject.name + " sees the player within activation distance.");
            AlertAllEnemies();
            ActivateChase();
        }
    }

    // Simple distance-based check without raycasting.
    private bool IsPlayerInRange()
    {
        if (player == null)
            return false;

        float distance = Vector3.Distance(player.position, transform.position);
        return distance <= activationDistance;
    }

    public static void AlertAllEnemies()
    {
        hiveMindAlert = true;
        foreach (EnemyDetection enemy in allEnemies)
        {
            enemy.ActivateChase();
        }
    }

    public static void ResetHiveMind()
    {
        hiveMindAlert = false;
        allEnemies.Clear();
    }

    public void ActivateChase()
    {
        if (movementScript != null)
            movementScript.enabled = true;
        // If you wish to use the A* pathfinding for chasing, enable it here:
        // if (aiPathScript != null)
        //     aiPathScript.enabled = true;
    }

    public void DeactivateChase()
    {
        if (movementScript != null)
            movementScript.enabled = false;
        if (aiPathScript != null)
            aiPathScript.enabled = false;
    }

    private void OnDisable()
    {
        if (allEnemies.Contains(this))
            allEnemies.Remove(this);
    }

    private void OnDestroy()
    {
        if (allEnemies.Contains(this))
            allEnemies.Remove(this);
    }
}
