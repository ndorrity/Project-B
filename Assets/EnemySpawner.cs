using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Enemy Prefab Variants")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    public int totalEnemiesToSpawn = 10;
    public float spawnRadius = 40f;
    public float minDistanceFromPlayer = 12f;

    [Header("Wave / Continuous")]
    public bool spawnContinuous = false;
    public float spawnInterval = 3f;

    private Transform playerTransform;
    private float nextSpawnTime = 0f;
    private int aliveEnemiesCount = 0;
    private bool allSpawned = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        SpawnBatch(totalEnemiesToSpawn);
        allSpawned = true;
    }

    void Update()
    {
        if (!spawnContinuous || Time.timeScale == 0f) return;

        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;
            SpawnSingleEnemy();
        }
    }

    public void SpawnBatch(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnSingleEnemy();
        }
    }

    public void SpawnSingleEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        Vector3 spawnPos;
        if (TryGetRandomNavMeshPoint(out spawnPos))
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);
            aliveEnemiesCount++;
        }
    }

    public void OnEnemyKilled()
    {
        aliveEnemiesCount--;
        Debug.Log("Enemy eliminated. Remaining enemies: " + aliveEnemiesCount);

        // Win condition: all spawned enemies cleared
        if (allSpawned && aliveEnemiesCount <= 0)
        {
            Debug.Log("All enemies eliminated! Displaying Game Over.");
            if (MenuManager.Instance != null)
            {
                MenuManager.Instance.ShowGameOver();
            }
        }
    }

    private bool TryGetRandomNavMeshPoint(out Vector3 result)
    {
        Vector3 center = playerTransform != null ? playerTransform.position : Vector3.zero;

        for (int i = 0; i < 30; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPoint = center + new Vector3(randomCircle.x, 0, randomCircle.y);

            if (playerTransform != null && Vector3.Distance(randomPoint, playerTransform.position) < minDistanceFromPlayer)
            {
                continue;
            }

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
}
