using UnityEditor;
using UnityEngine;

public class WelcomeUIController : MonoBehaviour
{
    public static WelcomeUIController instance;

    private void Awake()
    {
        instance = this;
    }

    public void OpenMainMenuScreen()
    {
        UIManager.instance.OpenMainMenuScreen();
    }
}
