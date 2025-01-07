using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("Welcome Screen Settings")]
    public GameObject welcomeScreen;
    [Space(10)]
    [Header("Main Menu Settings")]
    public GameObject mainMenuScreen;
    [Space(3)]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject powerUpPanel;
    public GameObject mapsPanel;
    [Space(10)]
    [Header("In-Game Settings")]
    public GameObject inGameScreen;
    [Space(3)]
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject levelUpPanel;
    public GameObject gameOverPanel;
    public GameObject resultsPanel;

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

    public void OpenWelcomeScreen()
    {
        CloseAll();
        welcomeScreen.SetActive(true);
    }

    public void OpenMainMenuScreen()
    {
        CloseAll();
        mainMenuScreen.SetActive(true);
        mainPanel.SetActive(true);
    }

    public void OpenInGameScreen()
    {
        CloseAll();
        inGameScreen.SetActive(true);
        hudPanel.SetActive(true);
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void OpenLevelUpPanel()
    {
        levelUpPanel.SetActive(true);
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void OpenResultsPanel()
    {
        resultsPanel.SetActive(true);
    }

    public void CloseAll()
    {
        welcomeScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        inGameScreen.SetActive(false);
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        powerUpPanel.SetActive(false);
        mapsPanel.SetActive(false);
        hudPanel.SetActive(false);
        pausePanel.SetActive(false);
        levelUpPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        resultsPanel.SetActive(false);
    }
}