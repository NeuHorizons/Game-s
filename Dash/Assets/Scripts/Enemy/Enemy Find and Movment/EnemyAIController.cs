using UnityEngine;
using Pathfinding;

public class EnemyAIController : MonoBehaviour
{
    public MonoBehaviour combatScript; 
    private AIPath aiPath; 

    private Transform player;
    public float attackRadius = 5f; 
    private bool usingPathfinding = false;
    private bool hasSpottedPlayer = false; 

    public static bool hiveMindAlert = false; 

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        aiPath.enabled = false; 
        if (combatScript != null)
        {
            combatScript.enabled = false; 
        }
    }

    private void Update()
    {
        if (!hiveMindAlert) return; 

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        
        if (distanceToPlayer > attackRadius && !usingPathfinding)
        {
            StartPathfinding();
        }
        
        else if (distanceToPlayer <= attackRadius && usingPathfinding)
        {
            StartCombat();
        }

        if (usingPathfinding && aiPath != null)
        {
            aiPath.destination = player.position; 
        }
    }

    public static void AlertAllEnemies()
    {
        hiveMindAlert = true; 
    }

    private void StartPathfinding()
    {
        usingPathfinding = true;
        if (combatScript != null)
        {
            combatScript.enabled = false; 
        }
        if (aiPath != null)
        {
            aiPath.enabled = true; 
        }
    }

    private void StartCombat()
    {
        usingPathfinding = false;
        if (aiPath != null)
        {
            aiPath.enabled = false; 
        }
        if (combatScript != null)
        {
            combatScript.enabled = true;
        }
    }
}
