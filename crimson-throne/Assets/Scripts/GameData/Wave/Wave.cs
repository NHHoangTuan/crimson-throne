using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "WaveSystem/Wave")]
public class Wave : ScriptableObject
{
    public string waveName = "Enemies Wave";
    public WaveType waveType = WaveType.NORMAL_WAVE;
    public float duration = 60;
    public int totalEnemies = 0;
    [Tooltip("No need if boss wave")]
    public float spawnInterval = 5f;
    [Tooltip("Just get the first one if boss wave")]
    public GameObject[] enemyPrefabs;
}

public enum WaveType
{
    NORMAL_WAVE,
    MINI_BOSS,
    FINAL_BOSS
}