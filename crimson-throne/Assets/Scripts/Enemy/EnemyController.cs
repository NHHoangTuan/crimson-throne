using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] protected float health;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected float dropExp;
    [SerializeField] protected bool isDefeated = false;
    [Header("Effects")]
    [SerializeField] protected GameObject deathEffect;
    protected bool isKnockedBack;
    protected Transform target;
    protected Animator animator;
    protected Rigidbody2D rb2d;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        target = PlayerController.instance.transform;
    }

    public virtual void Initialize(float health, float speed, float damage, int dropExp)
    {
        this.health = health;
        this.speed = speed;
        this.damage = damage;
        this.dropExp = dropExp;
    }

    protected virtual void Update()
    {
        if (isDefeated || target == null || isKnockedBack) return;
        MoveTowardsTarget();
    }

    protected virtual void MoveTowardsTarget()
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

    protected virtual void Die()
    {
        rb2d.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        isDefeated = true;
        GameManager.instance.UpdateKillsCount(1);
        if (Random.value <= 0.9f)
        {
            ItemSpawner.instance?.SpawnExp(dropExp, transform.position);
        }
        animator.SetTrigger("Dead");
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);
        }
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().ChangeHealth(-Mathf.FloorToInt(damage));
        }
    }
}
