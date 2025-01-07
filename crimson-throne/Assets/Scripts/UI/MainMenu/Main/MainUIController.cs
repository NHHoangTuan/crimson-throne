using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    public static MainUIController instance;

    private void Awake()
    {
        instance = this;
    }

    public void OpenMapsPanel()
    {
        UIManager.instance.mapsPanel.SetActive(true);
    }
}
