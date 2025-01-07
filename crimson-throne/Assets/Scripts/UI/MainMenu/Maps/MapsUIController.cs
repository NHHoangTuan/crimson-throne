using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapsUIController : MonoBehaviour
{
    public static MapsUIController instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadMap(int mapIndex)
    {
        UIManager.instance.OpenInGameScreen();
        switch (mapIndex)
        {
            case 0:
                Debug.Log("Loading Map 1");
                SceneManager.LoadScene("Map1");
                break;
            case 1:
                Debug.Log("Loading Map 2");
                SceneManager.LoadScene("Map2");
                break;
            case 2:
                Debug.Log("Loading Map 3");
                SceneManager.LoadScene("Map3");
                break;
            default:
                break;
        }
    }

    public void CloseMapsPanel()
    {
        UIManager.instance.mapsPanel.SetActive(false);
    }
}
