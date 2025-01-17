using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] protected float health = 1;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float dropExp = 1;
    [SerializeField] protected bool isDefeated = false;
    [SerializeField] protected bool isFinalBoss = false;
    [Header("Effects")]
    [SerializeField] protected GameObject deathEffect;
    protected bool isKnockedBack;
    protected Transform target;
    protected Animator animator;
    protected Rigidbody2D rb2d;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor; 
    protected AudioSource audioSource;

    protected System.Action<Vector2> spawnItemAction = null;

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
        if (PlayerController.instance != null)
        {
            target = PlayerController.instance.transform;
        }
    }

    public void SetSpawnItemAction(System.Action<Vector2> action)
    {
        if (action != null)
        {
            spawnItemAction = action;
        }
        else 
        {
            spawnItemAction = pos => ItemSpawner.instance?.SpawnExp(dropExp, pos);
        }
    }

    protected virtual void Update()
    {
        if (isDefeated || target == null || isKnockedBack) return;
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb2d.linearVelocity = direction * speed;
        if (direction.magnitude > 0)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public virtual void TakeDamage(float damageTaken, float knockbackForce)
    {
        if (isDefeated) return;
        animator.SetTrigger("Hit");
        health -= damageTaken;
        audioSource.PlayOneShot(AudioManager.instance.enemyHurt);
        StartCoroutine(FlashEffect());
        ApplyKnockback(knockbackForce);
        if (health <= 0)
        {
            if (isFinalBoss)
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
            Die();
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

    public void Die()
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().ChangeHealth(-Mathf.FloorToInt(damage));
        }
    }

    public void SetFinalBoss()
    {
        isFinalBoss = true;
    }
}