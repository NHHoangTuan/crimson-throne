using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossDash : MonoBehaviour
{
    public static BossDash instance { get; private set; }
    private Rigidbody2D rb2d;
    [SerializeField] private float cooldown = 7.5f;
    [SerializeField] private float force = 5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void StartAbility()
    {
        StartCoroutine(Dash());
    }
    
    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(cooldown);
        while (true)
        {
            if (PlayerController.instance == null) yield break;
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
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(AudioManager.instance.bossDash);

            yield return new WaitForSeconds(cooldown);
        }
    }
}