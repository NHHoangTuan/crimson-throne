using System.Collections;
using UnityEngine;

public class SpawnItemManager : MonoBehaviour
{
    #region Singleton
    public static SpawnItemManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region Variables
    [SerializeField] private Vector2 spawnBottomLeft;
    [SerializeField] private Vector2 spawnTopRight; 
    [SerializeField] private float initialSpawnDelay = 20f;
    [SerializeField] private float coinSpawnInterval = 12f;
    [SerializeField] private float healthSpawnInterval = 5f;
    [SerializeField] private int minCoinsPerSpawn = 4;
    [SerializeField] private int maxCoinsPerSpawn = 7;
    #endregion

    #region Initialization
    public void StartSpawnItems() 
    {
        if (!ValidateSpawnArea())
        {
            Debug.LogWarning("Invalid spawn area defined. Please check spawnBottomLeft and spawnTopRight.");
            return;
        }
        if (ItemSpawner.instance == null)
        {
            Debug.LogWarning("ItemSpawner instance is not set. SpawnItemManager cannot spawn items.");
            return;
        }
        StartCoroutine(SpawnItemsPeriodically());
    }
    
    private bool ValidateSpawnArea()
    {
        return spawnBottomLeft.x < spawnTopRight.x && spawnBottomLeft.y < spawnTopRight.y;
    }
    #endregion

    #region Spawn Controls
    private IEnumerator SpawnItemsPeriodically()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        while (true)
        {
            SpawnCoins();
            yield return new WaitForSeconds(coinSpawnInterval);

            SpawnHealthItem();
            yield return new WaitForSeconds(healthSpawnInterval);
        }
    }

    private void SpawnCoins()
    {
        int coinCount = Random.Range(minCoinsPerSpawn, maxCoinsPerSpawn);

        for (int i = 0; i < coinCount; i++)
        {
            Vector2 randomPosition = GetRandomPositionInRectangle();
            ItemSpawner.instance?.SpawnCoin(randomPosition);
        }
    }

    private void SpawnHealthItem()
    {
        Vector2 healthItemPosition = GetRandomPositionInRectangle();
        ItemSpawner.instance?.SpawnHealthItem(healthItemPosition);
    }

    private Vector2 GetRandomPositionInRectangle()
    {
        float randomX = Random.Range(spawnBottomLeft.x, spawnTopRight.x);
        float randomY = Random.Range(spawnBottomLeft.y, spawnTopRight.y);
        return new Vector2(randomX, randomY);
    }
    #endregion
}