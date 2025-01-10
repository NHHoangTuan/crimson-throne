using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public static UITimer instance { get; private set; }
    [SerializeField] private GameObject timerDialogBox;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetTimer(float timeElapsed)
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timerDialogBox.GetComponent<Text>().text = string.Format("{0:D1}:{1:D2}", minutes, seconds);
    }
}