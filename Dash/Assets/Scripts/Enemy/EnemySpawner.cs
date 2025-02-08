using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign enemy prefab in Unity
    public float spawnInterval = 5f; // Time between spawns
    public int maxEnemies = 5; // Max enemies at one time
    private int currentEnemies = 0;
    
    private bool isDestroyed = false; // If the beacon is destroyed, stop spawning

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (!isDestroyed)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Enemy enemyScript = enemyObj.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.SetSpawner(this); // Link the enemy to the spawner
        }

        currentEnemies++;
    }

    public void EnemyDied()
    {
        currentEnemies--; // Reduce enemy count when an enemy is destroyed
    }

    public void DestroySpawner()
    {
        isDestroyed = true;
        StopAllCoroutines();
        Destroy(gameObject); // Remove the spawner
    }
}