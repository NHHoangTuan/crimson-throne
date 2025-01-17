using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    #region Singleton
    public static WaveManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion
    
    #region Variables
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] public int maxEnemiesAlive = 150;
    [SerializeField] public bool canStart = true;
    private int enemiesAlive = 0;
    #endregion

    #region Initialization
    public void StartGame()
    {
        if (PlayerController.instance != null)
        {
            StartCoroutine(GameFlow());
        }
        else
        {
            Debug.LogWarning("Player not found! GameFlow will not start.");
        }
    }

    private IEnumerator GameFlow()
    {
        yield return new WaitForSeconds(1f);
        while (!canStart) 
        {
            yield return null;
        }
        foreach (Wave wave in waves)
        {
            yield return StartCoroutine(StartWave(wave));
            yield return new WaitForSeconds(5f);
        }
        Debug.Log("All waves have been spawned!");
    }
    #endregion

    #region Spawn Controls
    private IEnumerator StartWave(Wave wave)
    {
        Debug.Log($"Starting Wave: {wave.waveName}");

        switch (wave.waveType)
        {
            case WaveType.NORMAL_WAVE:
                float spawnInterval = wave.spawnInterval;
                int numSpawns = Mathf.CeilToInt(wave.duration / spawnInterval);
                int[] enemiesPerSpawn = DistributeEnemies(wave.totalEnemies, numSpawns);

                for (int i = 0; i < numSpawns; i++)
                {
                    if (enemiesAlive >= maxEnemiesAlive) yield return null;
                    SpawnMultipleEnemies(wave.enemyPrefabs, enemiesPerSpawn[i]);
                    yield return new WaitForSeconds(spawnInterval);
                }
                break;
            case WaveType.MINI_BOSS:
                for (int i = 0; i < wave.totalEnemies; i++)
                {
                    SpawnEnemy(wave.enemyPrefabs[0], null);
                }
                yield return new WaitForSeconds(wave.duration);
                break;
            case WaveType.FINAL_BOSS:
                if (wave.enemyPrefabs.Length > 0)
                {
                    SpawnEnemy(wave.enemyPrefabs[0], pos => ItemSpawner.instance?.SpawnGate(pos));
                }
                yield return new WaitForSeconds(wave.duration);
                break;
        }
    }

    private void SpawnEnemy(GameObject prefab, System.Action<Vector2> onDie)
    {
        Vector2 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController?.SetSpawnItemAction(onDie);
        enemiesAlive++;
    }

    private void SpawnMultipleEnemies(GameObject[] prefabs, int count)
    {
        if (prefabs.Length == 0) return;
        for (int i = 0; i < count; i++)
        {
            if (enemiesAlive >= maxEnemiesAlive) return;
            int randomIndex = Random.Range(0, prefabs.Length);
            SpawnEnemy(prefabs[randomIndex], null);
        }
    }

    public void EnemyDied()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }

    private int[] DistributeEnemies(int totalEnemies, int numSpawns)
    {
        int[] distribution = new int[numSpawns];
        float[] weights = new float[numSpawns];
        int mid = numSpawns / 2;
        for (int i = 0; i < numSpawns; i++)
        {
            int distanceToMid = Mathf.Abs(mid - i);
            weights[i] = 1f / (1 + distanceToMid);
        }
        float totalWeight = 0f;
        for (int i = 0; i < numSpawns; i++)
        {
            totalWeight += weights[i];
        }
        for (int i = 0; i < numSpawns; i++)
        {
            distribution[i] = Mathf.RoundToInt(totalEnemies * (weights[i] / totalWeight));
        }
        int currentTotal = 0;
        foreach (int count in distribution)
        {
            currentTotal += count;
        }
        int difference = totalEnemies - currentTotal;
        distribution[numSpawns - 1] += difference;
        return distribution;
    }
    #endregion
}