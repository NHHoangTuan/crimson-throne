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
    [Header("Effects")]
    [SerializeField] protected GameObject deathEffect;
    protected bool isKnockedBack;
    protected Transform target;
    protected Animator animator;
    protected Rigidbody2D rb2d;
    private System.Action<Vector2> spawnItemAction = null;

    private void Awake()
    {
        if (spawnItemAction == null)
        {
            spawnItemAction = pos => ItemSpawner.instance?.SpawnExp(dropExp, pos);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        target = PlayerController.instance.transform;
    }

    public void SetSpawnItemAction(System.Action<Vector2> action)
    {
        spawnItemAction = action;
    }

    private void Update()
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

    public void TakeDamage(float damageTaken, float knockbackForce)
    {
        if (isDefeated) return;
        animator.SetTrigger("Hit");
        health -= damageTaken;
        ApplyKnockback(knockbackForce);
        if (health <= 0)
        {
            Die();
        }
    }

    private void ApplyKnockback(float knockbackForce)
    {
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

    private void Die()
    {
        WaveManager.instance?.EnemyDied();
        rb2d.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        isDefeated = true;
        spawnItemAction?.Invoke(transform.position);

        // animator.SetTrigger("Dead");
        GameManager.instance.UpdateKillsCount(1);      
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
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
}