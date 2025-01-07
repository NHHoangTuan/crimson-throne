using System.Collections;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int coinsCount = 0;
    [SerializeField] private int killsCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        SkillsManager.instance.inactiveSkills[0].LevelUp();
        // WaveManager.instance.StartGame();
    }

    private void Update()
    {
    }

    public void GameOver()
    {
        // Handle game over logic
    }

    public void LoadNextLevel()
    {
        // Logic to load next scene
    }

    public void UpdateCoinsCount(int count)
    {
        coinsCount += count;
        UICollectItemCount.instance.SetCoinsCount(coinsCount);
    }

    public void UpdateKillsCount(int count)
    {
        killsCount += count;
        UICollectItemCount.instance.SetKillsCount(killsCount);
    }

    // private IEnumerator GameFlow()
    // {
    //     yield return StartCoroutine(stageManager.SpawnRandomAroundPlayer(20, 10f, enemyPrefabs));
    //     yield return new WaitForSeconds(5f); // Nghỉ giữa các stage

    //     yield return StartCoroutine(stageManager.SpawnWaveAcrossScreen(enemyPrefabs, new Vector3(-10, 0, 0), new Vector3(10, 0, 0), 10, 3f));
    //     yield return new WaitForSeconds(5f);

    //     yield return StartCoroutine(stageManager.SpawnCircleAroundPlayer(12, 5f, enemyPrefabs));
    // }
}
/*
public static GameManager Instance;

    public bool StopMoveing;
    public bool Pause;
    [Header("Player Stats")]
    public float speed;
    public float Damge;
    public float ExpBoost;
    public TMP_Text TextKill;
    public int NumberOfKills;
    public GameObject Panel;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameManger in scene");
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if(TextKill!=null)
        TextKill.text = NumberOfKills.ToString();
        if (Pause)
        {
            if(Panel!=null)
            Panel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            if (Panel != null)
                Panel.SetActive(false);
            Time.timeScale = 1;

        }

    }
*/