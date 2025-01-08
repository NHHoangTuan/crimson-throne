using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "WaveSystem/Wave")]
public class Wave : ScriptableObject
{
    public string waveName;
    public float duration;
    public int totalEnemies;
    public float spawnInterval = 5f;
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public bool hasBoss;
    public MapEventType mapEvent;
}

public enum MapEventType
{
    NONE,
    ENEMY_WALL,
    ENEMY_CIRCLE,
}