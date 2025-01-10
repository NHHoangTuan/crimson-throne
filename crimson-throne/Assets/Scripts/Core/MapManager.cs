using UnityEngine;
using System.IO;
using UnityEngine.PlayerLoop;
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
                UIManager.instance.OpenWelcomeScreen();
                break;
            case 1:
                // UI
                UIManager.instance.OpenInGameScreen();
                UIAbilities.instance.Reset();
                UIBackground.instance.PlayAnimation();
                // Audio
                AudioManager.instance.PlayMusic(AudioManager.instance.gameMusicLoopBackground);
                // Set Up Game Stats
                GameManager.instance?.StartNewGame();
                // Set Up Player
                GameObject player = Instantiate(GameManager.instance.player, spawnPoint.transform.position, Quaternion.identity);
                GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                if (mainCamera != null)
                {
                    CinemachineCamera cinemachineCamera = mainCamera.GetComponent<CinemachineCamera>();
                    if (cinemachineCamera != null && player != null)
                    {
                        cinemachineCamera.Follow = player.transform;
                    }
                }
                PlayerController.instance.gameObject.SetActive(true);
                PlayerAttributeBuffs.instance?.Reset();
                SkillsManager.instance.inactiveSkills[0].LevelUp();
                // Set Up Wave
                WaveManager.instance.StartGame();
                // Spawn Item (Health And Coins)
                SpawnItemManager.instance?.StartSpawnItems();
                break;
            case 2:
                PlayerController.instance.transform.position = spawnPoint.transform.position;
                WaveManager.instance.StartGame();
                SpawnItemManager.instance.StartSpawnItems();
                break;
            case 3:
                PlayerController.instance.transform.position = spawnPoint.transform.position;
                WaveManager.instance.StartGame();
                SpawnItemManager.instance.StartSpawnItems();
                break;
        }
    }
}