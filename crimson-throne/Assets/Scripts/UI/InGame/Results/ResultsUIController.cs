
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsUIController : MonoBehaviour
{
    public static ResultsUIController instance { get; private set; }
    [SerializeField] public TMP_Text titleText;
    [SerializeField] public TMP_Text timeText;
    [SerializeField] public TMP_Text killsText;
    [SerializeField] public TMP_Text coinsText;
    [SerializeField] public TMP_Text levelText;
   
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetUp(bool isVictory, string timePlayed, int killsCount, int coinsCount, int levelReached)
    {
        AudioManager.instance?.PlayMusic((isVictory == true) ? AudioManager.instance.victoryResultsBackground : AudioManager.instance.resultsBackground);
        Time.timeScale = 0f;
        UIManager.instance.resultsPanel.SetActive(true);
        titleText.text = isVictory ? "VICTORY" : "DEFEAT";
        timeText.text = timePlayed;
        killsText.text = killsCount.ToString();
        coinsText.text = coinsCount.ToString() + (isVictory ? "(x3)" : "");
        levelText.text = levelReached.ToString();
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins += coinsCount * (isVictory ? 3 : 1);
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
    }

    public void Close()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
        Time.timeScale = 1f;
        GameManager.instance.ReturnMainMenu();
    }
}