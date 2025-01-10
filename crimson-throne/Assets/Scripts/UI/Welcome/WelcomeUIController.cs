using UnityEditor;
using UnityEngine;

public class WelcomeUIController : MonoBehaviour
{
    public static WelcomeUIController instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void GoToMainMenuScreen()
    {
        UIManager.instance.OpenMainMenuScreen();
    }
}