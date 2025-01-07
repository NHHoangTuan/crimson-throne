using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudUIController : MonoBehaviour
{
    public static HudUIController instance;
    private float startTime = 0f; 
    public float currentTime = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        currentTime = startTime;
        isRunning = true;
        while (isRunning)
        {
            UpdateTimerUI();
            yield return new WaitForSeconds(1f);
            currentTime += 1f;
        }
    }

    private void UpdateTimerUI()
    {
        UIDialog.instance.SetTimer(currentTime);
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
