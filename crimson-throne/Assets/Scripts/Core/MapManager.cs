using UnityEngine;
using Unity.Cinemachine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance { private set; get; }
    [SerializeField] private GameObject spawnPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        switch (GameManager.instance?.currentScreenIndex)
        {
            case 0:
                SetupWelcomeScreen();
                break;
            case 1:
                SetupInGameScreen();
                break;
            case 2:
            case 3:
                SetupGameScreen(GameManager.instance.currentScreenIndex);
                break;
        }
    }

    private void SetupWelcomeScreen()
    {
        UIManager.instance?.OpenWelcomeScreen();
        AudioManager.instance?.PlayMusic(AudioManager.instance?.lofiOrchestraBackground);
    }

    private void SetupInGameScreen()
    {
        // UI
        UIManager.instance?.OpenInGameScreen();
        UIAbilities.instance?.Reset();
        UIStats.instance?.SetKillsCount(0);
        UIStats.instance?.SetCoinsCount(0);
        UITimer.instance?.SetTimer(0);
        UIExpBar.instance?.SetValue(0);
        UIExpBar.instance?.SetLevelText(0);
        // Audio
        AudioManager.instance?.PlayMusic(AudioManager.instance?.gameMusicLoopBackground);
        // Set Up Game Stats
        GameManager.instance?.StartNewGame();
        // Set Up Player
        GameObject player = Instantiate(GameManager.instance?.player, spawnPoint.transform.position, Quaternion.identity);
        SetupCamera(player);
        PlayerController.instance?.gameObject.SetActive(true);
        SkillsManager.instance?.inactiveSkills[0].LevelUp();
        // Set Up Wave
        WaveManager.instance?.StartGame();
        // Spawn Item (Health And Coins)
        SpawnItemManager.instance?.StartSpawnItems();
    }

    private void SetupGameScreen(int screenIndex)
    {
        // Audio
        AudioManager.instance?.PlayMusic(screenIndex == 2 ? AudioManager.instance.chillMusicBackground : AudioManager.instance.vampireSoundtrackBackground);
        // Set Up Player
        PlayerController.instance.transform.position = spawnPoint.transform.position;
        SetupCamera(PlayerController.instance?.gameObject);
        // Set Up Wave
        WaveManager.instance?.StartGame();
        // Spawn Item (Health And Coins)
        SpawnItemManager.instance?.StartSpawnItems();
    }

    private void SetupCamera(GameObject player)
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera != null)
        {
            CinemachineCamera cinemachineCamera = mainCamera.GetComponent<CinemachineCamera>();
            if (cinemachineCamera != null && player != null)
            {
                cinemachineCamera.Follow = player.transform;
            }
        }
    }
}
