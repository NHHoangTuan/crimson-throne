using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Variables
    public GameObject player;
    // ACHIEVEMENT
    [SerializeField] private int coinsCount = 0;
    [SerializeField] private int killsCount = 0;
    // TIME
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isPaused = false;
    private Coroutine timeCoroutine;
    // MAP
    [SerializeField] private List<string> screens = new List<string> { 
        "MainMenu", "Map1", "Map2", "Map3"
    }; 
    [SerializeField] public int currentScreenIndex = 0;
    #endregion

    #region Screens Controls
    public void NextLevel() 
    {
        if (currentScreenIndex < screens.Count - 1)
        {
            currentScreenIndex++;
            StartCoroutine(LoadSceneAsync(screens[currentScreenIndex]));
        }
    }

    public void ReturnMainMenu()
    {
        currentScreenIndex = 0;
        StartCoroutine(LoadSceneAsync(screens[currentScreenIndex]));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        UIBackground.instance?.Show();
        SetAudioState(true, 0f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        if (operation != null)
        {
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f && UIBackground.instance != null && UIBackground.instance.IsAnimationDone())
                {
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        SetAudioState(false, 1f);
        UIBackground.instance?.Hide();
    }

    private void SetAudioState(bool mute, float volume)
    {
        if (AudioManager.instance == null) return;
        AudioManager.instance.MuteSFX(mute);
        AudioManager.instance.SetMusicVolume(volume);
    }

    public bool IsFinalScreen()
    {
        return (currentScreenIndex == screens.Count - 1);
    }
    #endregion

    #region Stats Controls
    public void StartNewGame()
    {
        coinsCount = 0;
        killsCount = 0;
        currentTime = 0;
        isPaused = false;
        isRunning = true;
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
        }
        timeCoroutine = StartCoroutine(UpdateTimeCoroutine());
    }

    private IEnumerator UpdateTimeCoroutine()
    {
        while (isRunning)
        {
            if (!isPaused)
            {
                currentTime += Time.deltaTime;
                UITimer.instance?.SetTimer(currentTime);
            }
            yield return null;
        }
    }

    public void UpdateCoinsCount(int count)
    {
        coinsCount += count;
        UIStats.instance?.SetCoinsCount(coinsCount);
    }
    
    public void UpdateKillsCount(int count)
    {
        killsCount += count;
        UIStats.instance?.SetKillsCount(killsCount);
    }

    public void TogglePause(bool pause)
    {
        isPaused = pause;
    }
    #endregion

    #region End Game Controls
    public void EndGame(bool isVictory)
    {
        isRunning = false;
        Time.timeScale = 0f;
        ResultsUIController.instance?.SetUp(
            isVictory,
            string.Format("{0:D1}:{1:D2}", Mathf.FloorToInt(currentTime / 60), Mathf.FloorToInt(currentTime % 60)),
            killsCount,
            coinsCount,
            PlayerController.instance == null ? 0 : PlayerController.instance.getCurrentLevel()
        );
        
        CleanUpGame();
    }

    private void CleanUpGame()
    {
        PlayerController.instance?.DestroyCompletely();
        SkillsManager.instance?.DestroyCompletely();
        BuffsManager.instance?.DestroyCompletely();
    }
    #endregion
}