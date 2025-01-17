using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossDash : MonoBehaviour
{
    #region Singleton
    public static BossDash instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region Variables
    [SerializeField] private float cooldown = 7.5f;
    [SerializeField] private float force = 5f;
    private Rigidbody2D rb2d;
    private AudioClip dashSound;
    private AudioSource audioSource;
    #endregion

    #region Set Up
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        dashSound = AudioManager.instance.bossDash;
    }

    public void StartAbility()
    {
        StartCoroutine(Dash());
    }
        
    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(cooldown);
        if (PlayerController.instance == null) yield break;
        Transform playerTransform = PlayerController.instance.transform;
        while (true)
        {
            if (playerTransform == null) yield break;

            Vector2 direction = (PlayerController.instance.transform.position - transform.position).normalized;

            float angle = Random.Range(20f, 30f) * (Random.value > 0.5f ? 1 : -1);
            float radians = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            Vector2 rotatedDirection = new Vector2(
                direction.x * cos - direction.y * sin,
                direction.x * sin + direction.y * cos
            );

            float dashDuration = 0.3f;
            float elapsedTime = 0f;
            Vector2 initialPosition = rb2d.position;
            Vector2 targetPosition = initialPosition + rotatedDirection * force;

            while (elapsedTime < dashDuration)
            {
                rb2d.MovePosition(Vector2.Lerp(initialPosition, targetPosition, elapsedTime / dashDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rb2d.MovePosition(targetPosition);
            audioSource.PlayOneShot(dashSound);

            yield return new WaitForSeconds(cooldown);
        }
    }
    #endregion
}