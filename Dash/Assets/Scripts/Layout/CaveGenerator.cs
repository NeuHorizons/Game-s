using UnityEngine;
using System.Collections.Generic;

public class CaveGenerator : MonoBehaviour
{
    public int gridWidth = 50;
    public int gridHeight = 50;
    public float cellSize = 1f;

    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject roomCenterPrefab; 
    public GameObject playerPrefab;
   

    
    public Vector2Int spawnRoomPosition;
    public Vector2 dungeonOffset = Vector2.zero;
    public int fillPercentage = 45;
    public int smoothingIterations = 5;
    public int roomCount = 5;
    public int roomRadius = 3;
    public float roomEdgeNoise = 0.5f;
    private List<Vector2Int> roomCenters = new List<Vector2Int>(); 

    private int[,] grid;

    void Start()
    {
        GenerateRandomGrid();
        for (int i = 0; i < smoothingIterations; i++)
        {
            SmoothGrid();
        }
        CreateWiderRooms();
        ConnectDisconnectedCaves(); // Ensures tunnels are connected with at least 2 tiles width
        RenderGrid();
        SpawnRoomCenters();
        SpawnPlayer(playerPrefab);
    }

    void GenerateRandomGrid()
    {
        grid = new int[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1)
                {
                    grid[x, y] = 1;
                }
                else
                {
                    grid[x, y] = Random.Range(0, 100) < fillPercentage ? 1 : 0;
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
                if (wallCount > 4) newGrid[x, y] = 1;
                else if (wallCount < 4) newGrid[x, y] = 0;
                else newGrid[x, y] = grid[x, y];
            }
        }
        grid = newGrid;
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
                    if (grid[x, y] == 1) count++;
                }
            }
        }
        return count;
    }

    void CreateWiderRooms()
{
    roomCenters.Clear(); // Reset stored room centers

    // ✅ Step 1: Generate a Small, Round Spawn Room in an Isolated Area
    spawnRoomPosition = new Vector2Int(Random.Range(5, gridWidth - 5), Random.Range(5, gridHeight - 5));
    roomCenters.Add(spawnRoomPosition); // ✅ Mark this room as the spawn room

    int spawnRoomSize = 2; // Smaller, rounder spawn room

    for (int x = spawnRoomPosition.x - spawnRoomSize; x <= spawnRoomPosition.x + spawnRoomSize; x++)
    {
        for (int y = spawnRoomPosition.y - spawnRoomSize; y <= spawnRoomPosition.y + spawnRoomSize; y++)
        {
            if (x > 0 && y > 0 && x < gridWidth - 1 && y < gridHeight - 1)
            {
                float distance = Vector2Int.Distance(new Vector2Int(x, y), spawnRoomPosition);
                if (distance <= spawnRoomSize)
                {
                    grid[x, y] = 0; // ✅ Clear only a round area
                }
            }
        }
    }

    // ✅ Step 2: Generate Other Rooms (Ensuring They Don’t Touch the Spawn Room)
    for (int i = 0; i < roomCount; i++)
    {
        Vector2Int roomCenter;
        do
        {
            roomCenter = new Vector2Int(Random.Range(5, gridWidth - 5), Random.Range(5, gridHeight - 5));
        }
        while (Vector2Int.Distance(roomCenter, spawnRoomPosition) < (roomRadius * 3)); // Ensure rooms don't overlap the spawn

        roomCenters.Add(roomCenter);

        int maxRadius = Random.Range(roomRadius - 1, roomRadius + 2);

        for (int x = roomCenter.x - maxRadius; x <= roomCenter.x + maxRadius; x++)
        {
            for (int y = roomCenter.y - maxRadius; y <= roomCenter.y + maxRadius; y++)
            {
                if (x > 0 && y > 0 && x < gridWidth - 1 && y < gridHeight - 1)
                {
                    float distance = Vector2Int.Distance(new Vector2Int(x, y), roomCenter);
                    float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f) * roomEdgeNoise;
                    if (distance <= maxRadius + noise)
                    {
                        grid[x, y] = 0;
                    }
                }
            }
        }
    }
}



    void ConnectDisconnectedCaves()
    {
        List<List<Vector2Int>> caveRegions = GetCaveRegions();

        if (caveRegions.Count <= 1) return;

        List<Vector2Int> mainCave = caveRegions[0];

        for (int i = 1; i < caveRegions.Count; i++)
        {
            List<Vector2Int> otherCave = caveRegions[i];

            // ✅ Skip connecting the spawn room to the rest of the caves
            if (otherCave.Contains(spawnRoomPosition))
            {
                Debug.Log("🚫 Skipping connection for spawn room.");
                continue;
            }

            Vector2Int bestMainPoint = Vector2Int.zero;
            Vector2Int bestOtherPoint = Vector2Int.zero;
            float bestDistance = float.MaxValue;

            foreach (Vector2Int mainPoint in mainCave)
            {
                foreach (Vector2Int otherPoint in otherCave)
                {
                    float dist = Vector2Int.Distance(mainPoint, otherPoint);
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestMainPoint = mainPoint;
                        bestOtherPoint = otherPoint;
                    }
                }
            }

            DigTunnel(bestMainPoint, bestOtherPoint);
        }
    }

    
    void SpawnRoomCenters()
    {
        if (roomCenterPrefab == null) return;

        foreach (Vector2Int center in roomCenters)
        {
            // ✅ Skip the spawn room to prevent enemy spawners from appearing there
            if (center == spawnRoomPosition) 
            {
                Debug.Log("🚫 Skipping enemy spawner in spawn room.");
                continue;
            }

            Vector3 spawnPosition = new Vector3((center.x * cellSize) + dungeonOffset.x, (center.y * cellSize) + dungeonOffset.y, 0);
            Instantiate(roomCenterPrefab, spawnPosition, Quaternion.identity);
        }
    }



    List<List<Vector2Int>> GetCaveRegions()
    {
        List<List<Vector2Int>> caveRegions = new List<List<Vector2Int>>();
        bool[,] visited = new bool[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] == 0 && !visited[x, y])
                {
                    List<Vector2Int> region = FloodFill(new Vector2Int(x, y), visited);
                    caveRegions.Add(region);
                }
            }
        }
        return caveRegions;
    }

    List<Vector2Int> FloodFill(Vector2Int start, bool[,] visited)
    {
        List<Vector2Int> region = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            Vector2Int cell = queue.Dequeue();
            region.Add(cell);

            foreach (Vector2Int neighbor in GetNeighbors(cell))
            {
                if (!visited[neighbor.x, neighbor.y] && grid[neighbor.x, neighbor.y] == 0)
                {
                    visited[neighbor.x, neighbor.y] = true;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return region;
    }

    List<Vector2Int> GetNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = cell + dir;
            if (neighbor.x >= 0 && neighbor.y >= 0 && neighbor.x < gridWidth && neighbor.y < gridHeight)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    void DigTunnel(Vector2Int start, Vector2Int end)
    {
        Vector2Int current = start;

        while (current != end)
        {
            Vector3 worldPosition = new Vector3((current.x * cellSize) + dungeonOffset.x, (current.y * cellSize) + dungeonOffset.y, 0);
            grid[current.x, current.y] = 0; // Mark as floor

            // Ensure tunnel is at least 2 tiles wide by adding a perpendicular offset
            int offset = Random.Range(0, 2); // Either 0 or 1 to create variation

            if (current.x < end.x) { grid[current.x + offset, current.y] = 0; current.x++; }
            else if (current.x > end.x) { grid[current.x - offset, current.y] = 0; current.x--; }

            if (current.y < end.y) { grid[current.x, current.y + offset] = 0; current.y++; }
            else if (current.y > end.y) { grid[current.x, current.y - offset] = 0; current.y--; }
        }
    }


    void RenderGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3((x * cellSize) + dungeonOffset.x, (y * cellSize) + dungeonOffset.y, 0);
                GameObject prefabToSpawn = grid[x, y] == 1 ? wallPrefab : floorPrefab;
                Instantiate(prefabToSpawn, position, Quaternion.identity, transform);
            }
        }
    }
    void SpawnPlayer(GameObject playerPrefab)
    {
        if (playerPrefab == null) return;

        Vector3 spawnPosition = new Vector3(spawnRoomPosition.x * cellSize, spawnRoomPosition.y * cellSize, 0);
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }


    
}
