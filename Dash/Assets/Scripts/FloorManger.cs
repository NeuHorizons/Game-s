using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("Floor Tracking")]
    [Tooltip("The player's current floor in the dungeon.")]
    public int currentFloor = 1;
    [Tooltip("The highest floor the player has reached (for leaderboard purposes).")]
    public int highestFloorReached = 1;

    [Header("Player Data")]
    [Tooltip("Reference to the PlayerData ScriptableObject.")]
    public PlayerDataSO playerData;

    [Header("Level Layout Generator")]
    [Tooltip("Reference to the LevelLayoutGenerator, which builds the dungeon layout for the current floor.")]
    public LevelLayoutGenerator levelLayoutGenerator;

    [Header("Enemy Difficulty Settings")]
    [Tooltip("Enemy spawner components that will have their spawn rates and enemy stats adjusted based on the floor.")]
    public EnemySpawner[] enemySpawners;
    [Tooltip("Base spawn rate (seconds between spawns) at floor 1.")]
    public float baseSpawnRate = 5f;
    [Tooltip("Amount to decrease the spawn rate per floor (spawn rate gets lower as floor increases).")]
    public float spawnRateDecreasePerFloor = 0.1f;
    [Tooltip("Base enemy health at floor 1.")]
    public float baseEnemyHealth = 100f;
    [Tooltip("Amount to increase enemy health per floor.")]
    public float enemyHealthIncreasePerFloor = 10f;

    private void Start()
    {
        // Make sure PlayerData is assigned.
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned to FloorManager!");
            return;
        }

        // Initialize the highest floor from player data, if available.
        highestFloorReached = Mathf.Max(highestFloorReached, currentFloor);
        UpdatePlayerDataFloorInfo();

        // Inform the level layout generator of the initial floor.
        if (levelLayoutGenerator != null)
        {
            levelLayoutGenerator.GenerateLevel(currentFloor);
        }

        // Update enemy spawners for the starting floor.
        AdjustEnemyDifficulty();
    }

    /// <summary>
    /// Call this method when the player advances to a new floor.
    /// It updates the current floor, checks for a new highest floor,
    /// informs the level layout generator, and adjusts enemy difficulty.
    /// </summary>
    public void AdvanceToNextFloor()
    {
        currentFloor++;

        // Update highest floor reached if the new floor is greater.
        if (currentFloor > highestFloorReached)
        {
            highestFloorReached = currentFloor;
        }
        
        UpdatePlayerDataFloorInfo();

        // Inform the level layout generator to generate the new floor layout.
        if (levelLayoutGenerator != null)
        {
            levelLayoutGenerator.GenerateLevel(currentFloor);
        }

        // Adjust enemy spawner settings for the new floor.
        AdjustEnemyDifficulty();
    }

    /// <summary>
    /// Updates the PlayerData with the current and highest floor values.
    /// </summary>
    private void UpdatePlayerDataFloorInfo()
    {
        // Assuming your PlayerDataSO has fields to store these values.
        playerData.currentFloor = currentFloor;
        playerData.highestFloor = highestFloorReached;
    }

    /// <summary>
    /// Adjusts enemy difficulty parameters such as spawn rate and enemy health
    /// based on the current floor.
    /// </summary>
    private void AdjustEnemyDifficulty()
    {
        // Calculate new spawn rate (ensuring it doesn't drop below a minimum threshold).
        float newSpawnRate = Mathf.Max(1f, baseSpawnRate - (currentFloor * spawnRateDecreasePerFloor));
        // Calculate new enemy health.
        float newEnemyHealth = baseEnemyHealth + (currentFloor * enemyHealthIncreasePerFloor);

        // Update each enemy spawner with the new difficulty settings.
        if (enemySpawners != null)
        {
            foreach (EnemySpawner spawner in enemySpawners)
            {
                if (spawner != null)
                {
                    // Call the spawner's own method to update internal difficulty logic.
                    spawner.UpdateDifficulty(currentFloor);
                    // Also update the spawn rate and enemy health using FloorManager's computed values.
                    spawner.SetSpawnRate(newSpawnRate);
                    spawner.SetEnemyHealth(newEnemyHealth);
                }
            }
        }
    }
}
