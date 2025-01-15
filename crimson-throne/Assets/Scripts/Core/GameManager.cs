using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameObject player;

    // ACHIEVEMENT
    [SerializeField] private int coinsCount = 0;
    [SerializeField] private int killsCount = 0;
    
    // TIME
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isPaused = false;

    // MAP
    [SerializeField] private List<string> screens = new List<string> { 
        "MainMenu",
        "Map1",
        "Map2",
        "Map3",
    }; 
    [SerializeField] public int currentScreenIndex = 0;

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

    public void NextLevel() 
    {
        ++currentScreenIndex;
        if (currentScreenIndex >= screens.Count) return;
        
        StartCoroutine(LoadSceneAsync(screens[currentScreenIndex]));
    }

    public void ReturnMainMenu()
    {
        currentScreenIndex = 0;
        SceneManager.LoadScene(screens[currentScreenIndex]);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        UIBackground.instance?.Show();
        

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

        UIBackground.instance?.Hide();
    }

    public bool IsFinal()
    {
        return currentScreenIndex == screens.Count - 1;
    }

    public void StartNewGame()
    {
        coinsCount = 0;
        killsCount = 0;
        currentTime = 0;
        isPaused = false;
        StartCoroutine(StartTimer());
    }

    public void UpdateCoinsCount(int count)
    {
        UIStats.instance?.SetCoinsCount(coinsCount += count);
    }
    
    public void UpdateKillsCount(int count)
    {
        killsCount += count;
        UIStats.instance?.SetKillsCount(killsCount);
    }

    private IEnumerator StartTimer()
    {
        isRunning = true;
        while (isRunning)
        {
            UITimer.instance?.SetTimer(currentTime);
            yield return new WaitForSeconds(1f);
            if (!isPaused) currentTime += 1f;
        }
    }

    public void TogglePause(bool pause)
    {
        isPaused = pause;
    }

    public void EndGame(bool isVictory)
    {
        Time.timeScale = 0f;
        isRunning = false;
        ResultsUIController.instance?.SetUp(
            isVictory,
            string.Format("{0:D1}:{1:D2}", Mathf.FloorToInt(currentTime / 60), Mathf.FloorToInt(currentTime % 60)),
            killsCount,
            coinsCount,
            PlayerController.instance.getCurrentLevel()
        );
        PlayerController.instance?.DestroyCompletely();
        SkillsManager.instance?.DestroyCompletely();
        BuffsManager.instance?.DestroyCompletely();
    }
}