using UnityEngine;
using Unity.Cinemachine;

public class MapManager : MonoBehaviour
{
    #region Singleton
    public static MapManager instance { private set; get; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region Variables
    [SerializeField] private GameObject spawnPoint;
    #endregion

    #region Initialization
    private void Start()
    {
        int currentScreenIndex = GameManager.instance?.currentScreenIndex ?? -1;
        switch (currentScreenIndex)
        {
            case 0:
                SetupWelcomeScreen();
                break;
            case 1:
                SetupFirstGamePlay();
                AudioManager.instance?.PlayMusic(AudioManager.instance.gameMusicLoopBackground);
                break;
            case 2:
                SetupGamePlay();
                AudioManager.instance?.PlayMusic(AudioManager.instance.vampireSoundtrackBackground);
                break;
            case 3:
                SetupGamePlay();
                AudioManager.instance?.PlayMusic(AudioManager.instance.meetTheBossFinalMapBackground);
                WaveManager.instance.canStart = false;
                break;
            default:
                Debug.LogWarning($"Unhandled screen index: {currentScreenIndex}");
                break;
        }
    }
    #endregion

    #region Screen Setup Methods
    private void SetupWelcomeScreen()
    {
        UIManager.instance?.OpenWelcomeScreen();
        AudioManager.instance?.PlayMusic(AudioManager.instance.lofiOrchestraBackground);
    }

    private void SetupFirstGamePlay()
    {
        // UI
        UIManager.instance?.OpenInGameScreen();
        UIAbilities.instance?.Reset();
        UIStats.instance?.SetKillsCount(0);
        UIStats.instance?.SetCoinsCount(0);
        UITimer.instance?.SetTimer(0);
        UIExpBar.instance?.SetValue(0);
        UIExpBar.instance?.SetLevelText(0);
        // Set Up Game Stats
        GameManager.instance?.StartNewGame();
        // Set Up Player
        if (GameManager.instance?.player == null || spawnPoint == null)
        {
            Debug.LogWarning("Player or spawn point is not properly configured.");
            return;
        }
        GameObject player = Instantiate(GameManager.instance.player, spawnPoint.transform.position, Quaternion.identity);
        SetupCamera(player);
        if (PlayerController.instance == null || SkillsManager.instance == null) 
        {
            Debug.LogWarning("Cannot setup game due to lack of Player and Skill Managers!");
            return;
        }
        PlayerController.instance.gameObject.SetActive(true);
        SkillsManager.instance.SetDefaultSkill(PlayerController.instance.defaultSkill);
        SetUpSpawnLogic();
    }

    private void SetupGamePlay()
    {
        if (PlayerController.instance == null || spawnPoint == null)
        {
            Debug.LogWarning("Player or spawn point is not properly configured.");
            return;
        }
        // Set Up Player
        PlayerController.instance.transform.position = spawnPoint.transform.position;
        SetupCamera(PlayerController.instance.gameObject);
        SetUpSpawnLogic();
    }

    private void SetUpSpawnLogic()
    {
        // Set Up Wave
        WaveManager.instance?.StartGame();
        // Spawn Item (Health And Coins)
        SpawnItemManager.instance?.StartSpawnItems();
    }

    private void SetupCamera(GameObject player)
    {
        if (player == null)
        {
            Debug.LogWarning("Player is null. Cannot set up the camera.");
            return;
        }
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found.");
            return;
        }
        CinemachineCamera cinemachineCamera = mainCamera.GetComponent<CinemachineCamera>();
        if (cinemachineCamera != null && player != null)
        {
            cinemachineCamera.Follow = player.transform;
        }
        else
        {
            Debug.LogWarning("CinemachineCamera component not found on MainCamera.");
        }
    }
    #endregion
}