using UnityEngine;

public class Khoa_EnemyChase : MonoBehaviour
{
    public float speed = 4f;
    public Transform target;

    private Animator anim;

    private Rigidbody2D rb;

    float HorizontalFace;
    bool isFacingRight = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            rb.linearVelocity = direction * speed;

            HorizontalFace = direction.x; 

            FlipSprite();

            if (direction.magnitude > 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            anim.SetBool("isMoving", false);
        }
    }

    void FlipSprite()
    {
        if (HorizontalFace > 0f && !isFacingRight || HorizontalFace < 0f && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}