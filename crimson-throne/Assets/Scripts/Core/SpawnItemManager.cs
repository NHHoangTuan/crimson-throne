using System.Collections;
using UnityEngine;

public class SpawnItemManager : MonoBehaviour
{
    public static SpawnItemManager instance { get; private set; }
    [SerializeField] private Vector2 spawnBottomLeft;
    [SerializeField] private Vector2 spawnTopRight; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartSpawnItems() 
    {
        StartCoroutine(SpawnItemsPeriodically());
    }

    private IEnumerator SpawnItemsPeriodically()
    {
        while (true)
        {
            for (int i = 0; i < Random.Range(6,9); i++)
            {
                Vector2 randomPosition = GetRandomPositionInRectangle();
                ItemSpawner.instance.SpawnCoin(randomPosition);
                yield return new WaitForSeconds(5f);
            }
            yield return new WaitForSeconds(10f);
            Vector2 healthItemPosition = GetRandomPositionInRectangle();
            ItemSpawner.instance.SpawnHealthItem(healthItemPosition);
            yield return new WaitForSeconds(20f);
        }
    }

    private Vector2 GetRandomPositionInRectangle()
    {
        float randomX = Random.Range(spawnBottomLeft.x, spawnTopRight.x);
        float randomY = Random.Range(spawnBottomLeft.y, spawnTopRight.y);
        return new Vector2(randomX, randomY);
    }
}