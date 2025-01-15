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
        AudioManager.instance.PlaySFX(AudioManager.instance.startNewGame);
        GameManager.instance?.NextLevel();
    }

    public void OpenPowerUp()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
        UIManager.instance.mainPanel.SetActive(false);
        UIManager.instance.powerUpPanel.SetActive(true);
        PowerUpUIController.instance?.SetUp();
    }

    public void OpenSettings()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
        UIManager.instance.mainPanel.SetActive(false);
        UIManager.instance.settingsPanel.SetActive(true);
    }
}