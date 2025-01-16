using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossKatanaSpawner : MonoBehaviour
{
    public static BossKatanaSpawner instance { get; private set; }
    private Rigidbody2D rb2d;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float velocity = 6.5f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float damage = 10f;

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

    public float GetCurrentDamage()
    {
        return damage;
    }

    public void StartAbility()
    {
        StartCoroutine(SpawnBossKatanas());
    }
    
    private IEnumerator SpawnBossKatanas()
    {
        yield return new WaitForSeconds(cooldown);
        while (true)
        {
            if (PlayerController.instance == null) break;
            Vector2 direction = (PlayerController.instance.transform.position - transform.position).normalized;
            GameObject bossKatana = Instantiate(prefab, rb2d.position + direction * 0.2f, Quaternion.identity); 
            Rigidbody2D bossKatanaRb2d = bossKatana.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float roundedAngle = Mathf.Round(angle / 45f) * 45f;
                roundedAngle -= 90f;
                bossKatana.transform.rotation = Quaternion.Euler(0, 0, roundedAngle);
                bossKatanaRb2d.AddForce(direction * velocity, ForceMode2D.Impulse);
            }
            Destroy(bossKatana, duration);
            yield return new WaitForSeconds(cooldown);
        }
    }
}