using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance { get; private set; }
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private int maxEnemiesAlive = 150;
    private int enemiesAlive = 0;
    private Transform player;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        player = PlayerController.instance.transform;
    }

    public void StartGame()
    {
        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        foreach (Wave wave in waves)
        {
            yield return StartCoroutine(StartWave(wave));
            yield return new WaitForSeconds(5f);
        }
        Debug.Log("All waves completed!");
    }

    public IEnumerator StartWave(Wave wave)
    {
        Debug.Log($"Starting Wave: {wave.waveName}");
        
        float spawnInterval = wave.spawnInterval;
        int numSpawns = Mathf.CeilToInt(wave.duration / spawnInterval);
        Debug.Log($"Wave will spawn {numSpawns} times.");
        int[] enemiesPerSpawn = DistributeEnemies(wave.totalEnemies, numSpawns);

        for (int i = 0; i < numSpawns; i++)
        {
            if (enemiesAlive >= maxEnemiesAlive) yield return null;

            SpawnMultipleEnemies(wave.enemyPrefabs, enemiesPerSpawn[i]);
            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log($"Wave {wave.waveName} completed.");
    }

    private void SpawnMultipleEnemies(GameObject[] enemyPrefabs, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (enemiesAlive >= maxEnemiesAlive) break;

            Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            SpawnEnemy(enemyPrefab, spawnPosition.position);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab, Vector2 position)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity);
        enemiesAlive++;
    }

    private void SpawnBoss(GameObject bossPrefab)
    {
        if (bossPrefab != null)
        {
            Vector2 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Boss Spawned!");
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }

    private int[] DistributeEnemies(int totalEnemies, int numSpawns)
    {
        int[] distribution = new int[numSpawns];
        int midStart = Mathf.FloorToInt(numSpawns / 3);
        int midEnd = Mathf.CeilToInt(numSpawns * 2 / 3); 

        int remainingEnemies = totalEnemies;

        distribution[0] = Mathf.CeilToInt(totalEnemies * 0.1f);
        distribution[numSpawns - 1] = Mathf.CeilToInt(totalEnemies * 0.1f);

        remainingEnemies -= distribution[0] + distribution[numSpawns - 1];

        for (int i = midStart; i < midEnd; i++)
        {
            distribution[i] = Mathf.CeilToInt(remainingEnemies / (midEnd - midStart));
            remainingEnemies -= distribution[i];
        }

        for (int i = 1; i < numSpawns - 1; i++)
        {
            if (remainingEnemies <= 0) break;
            if (distribution[i] == 0)
            {
                distribution[i] = 1;
                remainingEnemies--;
            }
        }

        return distribution;
    }
}
// public void SpawnEnemy(GameObject enemyPrefab, Vector3 position)
// {
//     Instantiate(enemyPrefab, position, Quaternion.identity);
// }

// public void SpawnBoss(GameObject bossPrefab)
// {
//     if (bossPrefab != null)
//     {
//         Vector3 spawnPosition = GetRandomSpawnPosition();
//         Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
//         Debug.Log("Boss Spawned!");
//     }
// }

// private Vector3 GetRandomSpawnPosition()
// {
//     Vector3 spawnPosition;
//     Bounds colliderBounds = compositeCollider.bounds;

//     do
//     {
//         float x = Random.Range(colliderBounds.min.x, colliderBounds.max.x);
//         float y = Random.Range(colliderBounds.min.y, colliderBounds.max.y);
//         spawnPosition = new Vector3(x, y, 0);

//         // Kiểm tra vị trí có nằm trong CompositeCollider2D không
//     } while (!IsPositionInCompositeCollider(spawnPosition));

//     return spawnPosition;
// }

// private bool IsPositionInCompositeCollider(Vector3 position)
// {
//     // Kiểm tra vị trí có nằm trong collider của tilemap không
//     Collider2D hit = Physics2D.OverlapPoint(position, LayerMask.GetMask("SpawnArea"));
//     return hit != null && hit == compositeCollider;
// }

// public void TriggerMapEvent(string mapEvent)
// {
//     switch (mapEvent)
//     {
//         case "EnemyWall":
//             StartCoroutine(SpawnEnemyWall());
//             break;
//         case "EnemyCircle":
//             StartCoroutine(SpawnEnemyCircle());
//             break;
//         case "EnemyBlock":
//             StartCoroutine(SpawnEnemyBlock());
//             break;
//         default:
//             Debug.LogWarning($"Unknown map event: {mapEvent}");
//             break;
//     }
// }

// private IEnumerator SpawnEnemyWall()
// {
//     // Tạo tường quái vật
//     int enemyCount = 10;
//     for (int i = 0; i < enemyCount; i++)
//     {
//         Vector3 spawnPosition = GetRandomSpawnPosition();
//         SpawnEnemy(waves[currentWaveIndex].enemyPrefabs[Random.Range(0, waves[currentWaveIndex].enemyPrefabs.Length)], spawnPosition);
//         yield return new WaitForSeconds(0.5f);
//     }
// }

// private IEnumerator SpawnEnemyCircle()
// {
//     // Tạo vòng tròn quái vật
//     int enemyCount = 10;
//     float radius = 5f;

//     for (int i = 0; i < enemyCount; i++)
//     {
//         float angle = (i / (float)enemyCount) * Mathf.PI * 2;
//         Vector3 spawnPosition = player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
//         if (IsPositionInCompositeCollider(spawnPosition))
//         {
//             SpawnEnemy(waves[currentWaveIndex].enemyPrefabs[Random.Range(0, waves[currentWaveIndex].enemyPrefabs.Length)], spawnPosition);
//         }
//         yield return new WaitForSeconds(0.5f);
//     }
// }

// private IEnumerator SpawnEnemyBlock()
// {
//     // Tạo khối quái vật lướt qua người chơi
//     int enemyCount = 5;
//     Vector3 startPoint = GetRandomSpawnPosition();
//     Vector3 endPoint = new Vector3(startPoint.x + 10f, startPoint.y, 0);

//     for (int i = 0; i < enemyCount; i++)
//     {
//         GameObject enemy = Instantiate(waves[currentWaveIndex].enemyPrefabs[Random.Range(0, waves[currentWaveIndex].enemyPrefabs.Length)], startPoint, Quaternion.identity);
//         StartCoroutine(MoveEnemy(enemy, endPoint, 3f));
//         yield return new WaitForSeconds(0.5f);
//     }
// }

// private IEnumerator MoveEnemy(GameObject enemy, Vector3 endPoint, float speed)
// {
//     while (Vector3.Distance(enemy.transform.position, endPoint) > 0.1f)
//     {
//         enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, endPoint, speed * Time.deltaTime);
//         yield return null;
//     }
//     Destroy(enemy);
// }