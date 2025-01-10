using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GateItem : MonoBehaviour 
{
    private bool isPlayerInside = false;
    private Coroutine countdownCoroutine;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(StartCountdown());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
                StartCoroutine(RevertColor());
            }
        }
    }

    private IEnumerator StartCountdown()
    {
        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (!isPlayerInside) yield break; 

            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            spriteRenderer.color = Color.Lerp(originalColor, Color.red, progress);
            yield return null;
        }

        if (!GameManager.instance.IsFinal()) GameManager.instance.NextLevel();
    }

    private IEnumerator RevertColor()
    {
        float duration = 3f;
        float elapsedTime = 0f;
        Color currentColor = spriteRenderer.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            spriteRenderer.color = Color.Lerp(currentColor, originalColor, progress);
            yield return null;
        }
    }
}
