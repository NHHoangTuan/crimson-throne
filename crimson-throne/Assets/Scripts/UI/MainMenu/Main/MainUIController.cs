using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    public static MainUIController instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartNewGame()
    {
        GameManager.instance?.NextLevel();
    }

    public void OpenPowerUp()
    {
        UIManager.instance.mainPanel.SetActive(false);
        UIManager.instance.powerUpPanel.SetActive(true);
        PowerUpUIController.instance?.SetUp();
    }

    public void OpenSettings()
    {
        UIManager.instance.mainPanel.SetActive(false);
        UIManager.instance.settingsPanel.SetActive(true);
    }
}