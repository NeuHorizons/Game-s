using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public int gridWidth = 50;
    public int gridHeight = 50;
    public float cellSize = 1f;

    public GameObject floorPrefab;
    public GameObject wallPrefab;

    public int fillPercentage = 45; // % of initial walls for cellular automata
    public int smoothingIterations = 5; // Number of smoothing passes
    public int roomCount = 5; // Number of wider rooms
    public int roomRadius = 3; // Base radius of each room
    public float roomEdgeNoise = 0.5f; // Controls the jaggedness of room edges

    private int[,] grid;

    void Start()
    {
        GenerateRandomGrid();
        for (int i = 0; i < smoothingIterations; i++)
        {
            SmoothGrid();
        }
        CreateWiderRooms();
        RenderGrid();
    }

    void GenerateRandomGrid()
    {
        grid = new int[gridWidth, gridHeight];

        // Randomly fill the grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Keep edges as walls
                if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1)
                {
                    grid[x, y] = 1; // Wall
                }
                else
                {
                    grid[x, y] = Random.Range(0, 100) < fillPercentage ? 1 : 0; // 1 = Wall, 0 = Floor
                }
            }
        }
    }

    void SmoothGrid()
    {
        int[,] newGrid = new int[gridWidth, gridHeight];

        for (int x = 1; x < gridWidth - 1; x++)
        {
            for (int y = 1; y < gridHeight - 1; y++)
            {
                int wallCount = CountWallsAround(x, y);

                // Apply smoothing rules
                if (wallCount > 4)
                {
                    newGrid[x, y] = 1; // Wall
                }
                else if (wallCount < 4)
                {
                    newGrid[x, y] = 0; // Floor
                }
                else
                {
                    newGrid[x, y] = grid[x, y]; // Keep current state
                }
            }
        }

        grid = newGrid; // Update the grid
    }

    int CountWallsAround(int gridX, int gridY)
    {
        int count = 0;

        for (int x = gridX - 1; x <= gridX + 1; x++)
        {
            for (int y = gridY - 1; y <= gridY + 1; y++)
            {
                if (x >= 0 && y >= 0 && x < gridWidth && y < gridHeight)
                {
                    if (grid[x, y] == 1)
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    void CreateWiderRooms()
    {
        for (int i = 0; i < roomCount; i++)
        {
            // Pick a random position inside the grid
            int roomX = Random.Range(1, gridWidth - 1);
            int roomY = Random.Range(1, gridHeight - 1);

            // Add variation to the room shape
            int maxRadius = Random.Range(roomRadius - 1, roomRadius + 2); // Slightly vary the room radius

            for (int x = roomX - maxRadius; x <= roomX + maxRadius; x++)
            {
                for (int y = roomY - maxRadius; y <= roomY + maxRadius; y++)
                {
                    if (x > 0 && y > 0 && x < gridWidth - 1 && y < gridHeight - 1)
                    {
                        // Calculate distance for room edge
                        float distance = Mathf.Sqrt((x - roomX) * (x - roomX) + (y - roomY) * (y - roomY));

                        // Add noise-based protrusions
                        float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f) * roomEdgeNoise;

                        // Allow rocky protrusions but blend with existing terrain
                        if (distance <= maxRadius + noise)
                        {
                            // Blend room edges with tunnels
                            if (grid[x, y] == 1) // If it's a wall, allow carving
                            {
                                grid[x, y] = 0; // Carve to floor
                            }
                        }
                    }
                }
            }
        }
    }


    void RenderGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject prefabToSpawn = grid[x, y] == 1 ? wallPrefab : floorPrefab;
                Instantiate(prefabToSpawn, position, Quaternion.identity, transform);
            }
        }
    }
}
