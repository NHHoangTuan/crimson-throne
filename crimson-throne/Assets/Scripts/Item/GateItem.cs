using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GateItem : MonoBehaviour 
{
    #region Variables
    private Coroutine countdownCoroutine;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    #endregion

    #region Controls
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(StartCountdown());
            }
        }
    }

    private IEnumerator StartCountdown()
    {
        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            spriteRenderer.color = Color.Lerp(originalColor, Color.red, progress);
            yield return null;
        }

        if (!GameManager.instance.IsFinalScreen()) GameManager.instance?.NextLevel();
    }
    #endregion
}