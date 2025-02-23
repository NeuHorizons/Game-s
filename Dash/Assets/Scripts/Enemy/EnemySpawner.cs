using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public float spawnChance;
    }

    public List<EnemySpawnData> enemyList = new List<EnemySpawnData>();
    public float spawnInterval = 5f;
    public int maxEnemies = 5;
    public float spawnRadius = 3f; // Radius around the spawner for random spawning

    private int currentEnemies = 0;
    private bool isDestroyed = false;

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
        if (enemyList.Count == 0) return;

        GameObject selectedEnemy = GetRandomEnemy();

        if (selectedEnemy != null)
        {
            Vector3 spawnPosition = GetRandomSpawnPoint();
            GameObject enemyObj = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);
            Enemy enemyScript = enemyObj.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.SetSpawner(this);
            }

            currentEnemies++;
        }
    }

    Vector3 GetRandomSpawnPoint()
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius; // Get random point within a circle
        Vector3 randomSpawnPosition = new Vector3(transform.position.x + randomOffset.x, transform.position.y + randomOffset.y, transform.position.z);
        return randomSpawnPosition;
    }

    GameObject GetRandomEnemy()
    {
        float totalWeight = 0f;

        foreach (var enemy in enemyList)
        {
            totalWeight += enemy.spawnChance;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var enemy in enemyList)
        {
            cumulativeWeight += enemy.spawnChance;
            if (randomValue <= cumulativeWeight)
            {
                return enemy.enemyPrefab;
            }
        }

        return null;
    }

    public void EnemyDied()
    {
        currentEnemies--;
    }

    public void DestroySpawner()
    {
        isDestroyed = true;
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
