using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load scene chính
        //SceneManager.LoadScene("GameScene");

        SceneManager.LoadScene(1);
    }

    public void OpenPowerUp()
    {
        // Chuyển đến màn hình Power Up
        Debug.Log("Power Up Menu Opened");
    }

    public void OpenCollection()
    {
        // Chuyển đến màn hình Collection
        Debug.Log("Collection Menu Opened");
    }

    public void OpenSettings()
    {
        // Mở Settings Panel
        Debug.Log("Settings Menu Opened");
    }
}
