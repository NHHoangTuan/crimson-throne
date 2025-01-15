
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
        Time.timeScale = 0f;
        UIManager.instance.resultsPanel.SetActive(true);
        titleText.text = isVictory ? "VICTORY" : "DEFEAT";
        timeText.text = timePlayed;
        killsText.text = killsCount.ToString() + (isVictory ? "(x3)" : "");
        coinsText.text = coinsCount.ToString();
        levelText.text = levelReached.ToString();
        if (isVictory) 
        {
            PlayerPrefs.SetInt("TotalCoins", coinsCount * (isVictory ? 3 : 1));
            PlayerPrefs.Save();
        }
    }

    public void Close()
    {
        Time.timeScale = 1f;
        GameManager.instance.ReturnMainMenu();
    }
}