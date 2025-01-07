using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{
    public static UIDialog instance { get; private set; }
    [SerializeField] private GameObject levelTextDialogBox;
    [SerializeField] private GameObject timerDialogBox;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetLevelText(int level)
    {
        levelTextDialogBox.GetComponent<Text>().text = "Level " + level;
    }
    
    public void SetTimer(float timeElapsed)
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timerDialogBox.GetComponent<Text>().text = string.Format("{0:D1}:{1:D2}", minutes, seconds);
    }
}

