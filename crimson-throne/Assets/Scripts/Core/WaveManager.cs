using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance { get; private set; }
    [SerializeField] private Wave[] waves;
    [SerializeField] private Tilemap spawnTilemap; // Tilemap sử dụng để làm vùng spawn
    private CompositeCollider2D compositeCollider; // Collider từ Tilemap
    private Transform player;
    private int currentWaveIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        player = PlayerController.instance.transform;
        compositeCollider = spawnTilemap.GetComponent<CompositeCollider2D>();
        if (compositeCollider == null)
        {
            Debug.LogError("CompositeCollider2D is missing from the Tilemap!");
        }
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
            yield return new WaitForSeconds(5f); // Thời gian nghỉ giữa các wave
        }
        Debug.Log("All Waves Completed!");
    }

    public IEnumerator StartWave(Wave wave)
    {
        Debug.Log($"Starting Wave: {wave.waveName}");
        float elapsedTime = 0f;
        int totalSpawnedEnemies = 0;

        // Spawn boss sớm nếu có
        if (wave.hasBoss)
        {
            SpawnBoss(wave.bossPrefab);
        }

        // Tách wave thành các đợt spawn
        while (elapsedTime < wave.duration)
        {
            // Đảm bảo số lượng quái spawn từ đầu đến cuối wave
            float adjustedSpawnInterval = Mathf.Max(0.5f, wave.spawnInterval - (elapsedTime / wave.duration));

            // Số lượng quái spawn trong đợt này
            int enemiesToSpawn = Mathf.CeilToInt(wave.totalEnemies * (1f - (elapsedTime / wave.duration)));

            // Spawn quái vật từ danh sách prefab
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if (totalSpawnedEnemies >= wave.totalEnemies) break;

                Vector2 spawnPosition = GetRandomSpawnPosition();
                SpawnEnemy(wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)], spawnPosition);
                totalSpawnedEnemies++;
                yield return new WaitForSeconds(adjustedSpawnInterval);
            }

            elapsedTime += adjustedSpawnInterval;
            yield return null; // Đảm bảo frame được cập nhật
        }

        // Trigger map event nếu có
        if (wave.hasMapEvent)
        {
            TriggerMapEvent(wave.mapEvent);
        }

        Debug.Log($"Wave {wave.waveName} completed.");
    }

    public void SpawnEnemy(GameObject enemyPrefab, Vector3 position)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    public void SpawnBoss(GameObject bossPrefab)
    {
        if (bossPrefab != null)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Boss Spawned!");
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        Bounds colliderBounds = compositeCollider.bounds;

        do
        {
            float x = Random.Range(colliderBounds.min.x, colliderBounds.max.x);
            float y = Random.Range(colliderBounds.min.y, colliderBounds.max.y);
            spawnPosition = new Vector3(x, y, 0);

            // Kiểm tra vị trí có nằm trong CompositeCollider2D không
        } while (!IsPositionInCompositeCollider(spawnPosition));

        return spawnPosition;
    }

    private bool IsPositionInCompositeCollider(Vector3 position)
    {
        // Kiểm tra vị trí có nằm trong collider của tilemap không
        Collider2D hit = Physics2D.OverlapPoint(position, LayerMask.GetMask("SpawnArea"));
        return hit != null && hit == compositeCollider;
    }

    public void TriggerMapEvent(string mapEvent)
    {
        switch (mapEvent)
        {
            case "EnemyWall":
                StartCoroutine(SpawnEnemyWall());
                break;
            case "EnemyCircle":
                StartCoroutine(SpawnEnemyCircle());
                break;
            case "EnemyBlock":
                StartCoroutine(SpawnEnemyBlock());
                break;
            default:
                Debug.LogWarning($"Unknown map event: {mapEvent}");
                break;
        }
    }

    private IEnumerator SpawnEnemyWall()
    {
        // Tạo tường quái vật
        int enemyCount = 10;
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            SpawnEnemy(waves[currentWaveIndex].enemyPrefabs[Random.Range(0, waves[currentWaveIndex].enemyPrefabs.Length)], spawnPosition);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator SpawnEnemyCircle()
    {
        // Tạo vòng tròn quái vật
        int enemyCount = 10;
        float radius = 5f;

        for (int i = 0; i < enemyCount; i++)
        {
            float angle = (i / (float)enemyCount) * Mathf.PI * 2;
            Vector3 spawnPosition = player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            if (IsPositionInCompositeCollider(spawnPosition))
            {
                SpawnEnemy(waves[currentWaveIndex].enemyPrefabs[Random.Range(0, waves[currentWaveIndex].enemyPrefabs.Length)], spawnPosition);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator SpawnEnemyBlock()
    {
        // Tạo khối quái vật lướt qua người chơi
        int enemyCount = 5;
        Vector3 startPoint = GetRandomSpawnPosition();
        Vector3 endPoint = new Vector3(startPoint.x + 10f, startPoint.y, 0);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(waves[currentWaveIndex].enemyPrefabs[Random.Range(0, waves[currentWaveIndex].enemyPrefabs.Length)], startPoint, Quaternion.identity);
            StartCoroutine(MoveEnemy(enemy, endPoint, 3f));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator MoveEnemy(GameObject enemy, Vector3 endPoint, float speed)
    {
        while (Vector3.Distance(enemy.transform.position, endPoint) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, endPoint, speed * Time.deltaTime);
            yield return null;
        }
        Destroy(enemy);
    }
}
