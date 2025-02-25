using UnityEngine;
using Pathfinding;

public class EnemyAIController : MonoBehaviour
{
    public MonoBehaviour movementScript; // ✅ This is the Combat Movement Script
    private AIPath aiPath; // ✅ A* Pathfinding component

    private Transform player;
    public float attackRadius = 5f; // ✅ Distance where enemy switches to Movement Script
    private bool usingPathfinding = false;

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        aiPath.enabled = false; // ✅ Disable A* initially
        if (movementScript != null)
            movementScript.enabled = false; // ✅ Disable Movement Script initially
    }

    private void Update()
    {
        if (!EnemyDetection.hiveMindAlert) return; // ✅ Stop if no alert

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ✅ If enemy is OUTSIDE attack range, use Pathfinding (A*)
        if (distanceToPlayer > attackRadius && !usingPathfinding)
        {
            StartPathfinding();
        }
        // ✅ If enemy is CLOSE to the player, switch to Movement Script
        else if (distanceToPlayer <= attackRadius && usingPathfinding)
        {
            StartCombatMovement();
        }

        if (usingPathfinding && aiPath != null)
        {
            aiPath.destination = player.position; // ✅ Keep updating A* path
        }
    }

    private void StartPathfinding()
    {
        usingPathfinding = true;
        if (movementScript != null)
            movementScript.enabled = false; // ✅ Disable Movement Script (Combat Mode)

        if (aiPath != null)
            aiPath.enabled = true; // ✅ Enable A* pathfinding
    }

    private void StartCombatMovement()
    {
        usingPathfinding = false;
        if (aiPath != null)
            aiPath.enabled = false; // ✅ Disable A*

        if (movementScript != null)
            movementScript.enabled = true; // ✅ Enable Movement Script (Combat Mode)
    }
}
