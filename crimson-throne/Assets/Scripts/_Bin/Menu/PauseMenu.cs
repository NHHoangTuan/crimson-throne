using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button quitButton;
    public string mainMenuSceneName = "MainMenu"; // Tên scene của Main Menu

    void Start()
    {
        // Gán chức năng cho các button
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(QuitToMainMenu);
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (GameIsPaused)
            {
                NextScene();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Đặt timescale về 1 để game tiếp tục chạy
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Đặt timescale về 0 để game dừng lại
        GameIsPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Trả timescale về 1 trước khi load scene
        GameIsPaused = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void NextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Scene Index: " + currentSceneIndex);
        if (currentSceneIndex >= 3) return;
        Time.timeScale = 1f; // Trả timescale về 1 trước khi load scene
        GameIsPaused = false;
        // Chuyển sang Scene tiếp theo dựa trên Build Index
        
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}