using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    #region Variables
    [Header("Enemy Attributes")]
    [SerializeField] protected float health = 1;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float dropExp = 1;
    [SerializeField] protected bool isDefeated = false;
    [SerializeField] protected bool killAll = false;
    [Header("Effects")]
    [SerializeField] protected GameObject deathEffect;
    protected bool isKnockedBack;
    protected Transform target;
    protected Animator animator;
    protected Rigidbody2D rb2d;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor; 
    protected AudioSource audioSource;
    // DEATH HANDLE
    protected System.Action<Vector2> spawnItemAction = null;
    #endregion

    #region Initialization
    private void Awake()
    {
        if (spawnItemAction == null)
        {
            spawnItemAction = pos => ItemSpawner.instance?.SpawnExp(dropExp, pos);
        }
    }

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        target = PlayerController.instance?.transform;
    }

    public void SetSpawnItemAction(System.Action<Vector2> action)
    {
        spawnItemAction = action ?? (pos => ItemSpawner.instance?.SpawnExp(dropExp, pos));
    }
    #endregion

    #region Moving Controls
    protected virtual void Update()
    {
        if (isDefeated || target == null || isKnockedBack) return;
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb2d.linearVelocity = direction * speed;
        animator.SetBool("isMoving", direction.magnitude > 0);
        animator.SetFloat("moveX", direction.x);
    }
    #endregion

    #region Take Damage Controls
    public virtual void TakeDamage(float damageTaken, float knockbackForce)
    {
        if (isDefeated) return;
        health -= damageTaken;
        audioSource.PlayOneShot(AudioManager.instance.enemyHurt);
        StartCoroutine(FlashEffect());
        if (health <= 0)
        {
            if (killAll)
            {
                PlayerController.instance?.ChangeHealth(100);
                KillAllEnemies();
            }
            else 
            {
                Die();
            }
        }
        else
        {
            ApplyKnockback(knockbackForce);
        }
    }

    private IEnumerator FlashEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ApplyKnockback(float knockbackForce)
    {
        if (target == null) return;

        Vector2 knockbackDirection = (transform.position - PlayerController.instance.transform.position).normalized;
        StartCoroutine(HandleKnockback(knockbackDirection, knockbackForce));
    }

    private IEnumerator HandleKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        isKnockedBack = true;
        rb2d.linearVelocity = Vector2.zero;
        rb2d.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); 
        yield return new WaitForSeconds(0.1f);
        rb2d.linearVelocity = Vector2.zero; 
        isKnockedBack = false;
    }

    private void KillAllEnemies()
    {
        WaveManager.instance.maxEnemiesAlive = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null && !enemyController.isDefeated)
            {
                enemyController.Die();
            }
        }
    }

    public virtual void Die()
    {
        WaveManager.instance?.EnemyDied();
        rb2d.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        isDefeated = true;

        spawnItemAction?.Invoke(transform.position);
        GameManager.instance?.UpdateKillsCount(1);      
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            AudioSource effectAudioSource = effect.GetComponent<AudioSource>();
            effectAudioSource.PlayOneShot(AudioManager.instance.enemyHurt);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        }
        Destroy(gameObject);
    }
    #endregion

    #region Attack Controls
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().ChangeHealth(-Mathf.FloorToInt(damage));
        }
    }
    #endregion
}