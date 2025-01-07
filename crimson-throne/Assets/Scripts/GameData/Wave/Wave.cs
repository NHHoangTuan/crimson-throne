using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Wave System/Wave")]
public class Wave : ScriptableObject
{
    public string waveName;
    public float duration;
    public int totalEnemies;
    public float spawnInterval;
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public bool hasBoss;
    public bool hasMapEvent;
    [Tooltip("EnemyWall / EnemyCircle / EnemyBlock")]
    public string mapEvent;
}
