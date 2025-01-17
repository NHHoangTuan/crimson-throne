using System.Collections;
using UnityEngine;

public class Cage : MonoBehaviour
{
    #region Variables
    private bool isKeyDetected = false; 
    private float fadeDuration = 3f;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region Controls
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<KeyItem>() != null && !isKeyDetected)
        {
            isKeyDetected = true;
            StartCoroutine(FadeOutAndVictory());
        }
    }

    private IEnumerator FadeOutAndVictory()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration); 
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
            yield return null; 
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0, 0, 0, 0);
        }
        
        yield return new WaitForSeconds(2f);
        GameManager.instance?.EndGame(true);
    }
    #endregion
}