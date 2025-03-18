using UnityEngine;

public class LevelLayoutGenerator : MonoBehaviour
{
    [Header("Prefabs & Layouts")]
    [Tooltip("Prefab for the standard dungeon layout.")]
    public GameObject standardLayoutPrefab;
    [Tooltip("Prefab for a boss level layout.")]
    public GameObject bossLayoutPrefab;
    
    [Header("Generation Options")]
    [Tooltip("If true, generate a random layout for non-milestone floors. Otherwise, load a pre-designed layout.")]
    public bool useRandomGeneration = true;

    /// <summary>
    /// Generates the level layout based on the current floor number.
    /// For milestone floors (e.g., every 10th floor), a boss layout is generated.
    /// Otherwise, either a random layout or a pre-designed layout is loaded.
    /// </summary>
    /// <param name="currentFloor">The current floor level.</param>
    public void GenerateLevel(int currentFloor)
    {
        Debug.Log("Generating layout for floor: " + currentFloor);

        // For milestone floors (e.g., every 10th floor) generate a boss layout.
        if (currentFloor % 10 == 0)
        {
            GenerateBossLayout(currentFloor);
        }
        else
        {
            if (useRandomGeneration)
            {
                GenerateRandomLayout(currentFloor);
            }
            else
            {
                LoadPreDesignedLayout(currentFloor);
            }
        }
    }

    /// <summary>
    /// Generates a random dungeon layout for the given floor.
    /// </summary>
    /// <param name="currentFloor">The current floor level.</param>
    private void GenerateRandomLayout(int currentFloor)
    {
        Debug.Log("Generating random layout for floor: " + currentFloor);

        if (standardLayoutPrefab != null)
        {
            // Here you might add custom logic for randomization.
            Instantiate(standardLayoutPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No standard layout prefab assigned!");
        }
    }

    /// <summary>
    /// Generates a boss level layout for milestone floors.
    /// </summary>
    /// <param name="currentFloor">The current floor level.</param>
    private void GenerateBossLayout(int currentFloor)
    {
        Debug.Log("Generating boss layout for floor: " + currentFloor);

        if (bossLayoutPrefab != null)
        {
            Instantiate(bossLayoutPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No boss layout prefab assigned!");
        }
    }

    /// <summary>
    /// Loads a pre-designed layout for the given floor.
    /// This assumes you have predesigned layouts stored in Resources under "PreDesignedLayouts".
    /// </summary>
    /// <param name="currentFloor">The current floor level.</param>
    private void LoadPreDesignedLayout(int currentFloor)
    {
        Debug.Log("Loading pre-designed layout for floor: " + currentFloor);

        // Example: Layouts are stored in Resources/PreDesignedLayouts/Floor_X where X is the floor number.
        GameObject layoutPrefab = Resources.Load<GameObject>("PreDesignedLayouts/Floor_" + currentFloor);
        if (layoutPrefab != null)
        {
            Instantiate(layoutPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No pre-designed layout found for floor: " + currentFloor);
        }
    }
}
