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
    [SerializeField] private int maxCoins = 15;
    [SerializeField] private int maxHealthItems = 3;
    [SerializeField] private bool canSpawn = true;
    private int currentCoinCount = 0; 
    private int currentHealthItemCount = 0;
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
        while (canSpawn)
        {
            SpawnCoins();
            yield return new WaitForSeconds(coinSpawnInterval);

            SpawnHealthItem();
            SpawnHealthItem();
            yield return new WaitForSeconds(healthSpawnInterval);
        }
    }

    private void SpawnCoins()
    {
        if (currentCoinCount >= maxCoins)
        {
            return;
        }
        int coinCount = Random.Range(minCoinsPerSpawn, maxCoinsPerSpawn);
        coinCount = Mathf.Min(coinCount, maxCoins - currentCoinCount);

        for (int i = 0; i < coinCount; i++)
        {
            Vector2 randomPosition = GetRandomPositionInRectangle();
            ItemSpawner.instance?.SpawnCoin(randomPosition);
            currentCoinCount++;
        }
    }

    private void SpawnHealthItem()
    {
        if (currentHealthItemCount >= maxHealthItems)
        {
            return;
        }
        Vector2 healthItemPosition = GetRandomPositionInRectangle();
        ItemSpawner.instance?.SpawnHealthItem(healthItemPosition);
        currentHealthItemCount++;
    }

    private Vector2 GetRandomPositionInRectangle()
    {
        float randomX = Random.Range(spawnBottomLeft.x, spawnTopRight.x);
        float randomY = Random.Range(spawnBottomLeft.y, spawnTopRight.y);
        return new Vector2(randomX, randomY);
    }

    private void TurnOffSpawning()
    {
        canSpawn = false;
    }
    #endregion

    #region Decrease Item Counts
    public void OnCoinCollected()
    {
        if (currentCoinCount > 0)
        {
            currentCoinCount--;
        }
    }

    public void OnHealthItemCollected()
    {
        if (currentHealthItemCount > 0)
        {
            currentHealthItemCount--;
        }
    }
    #endregion
}