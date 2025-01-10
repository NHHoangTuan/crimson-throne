// using UnityEngine;

// public class ChasePlayer : MonoBehaviour
// {
//     public float speed = 2f;
//     public Transform target; // Kéo thả GameObject Player vào đây trong Inspector
//     private Animator anim;
//     private Rigidbody2D rb;

//     void Start()
//     {
//         anim = GetComponent<Animator>();
//         rb = GetComponent<Rigidbody2D>();
//     }

//     void Update()
//     {
//         if (target != null)
//         {
//             // Tính toán hướng di chuyển đến người chơi
//             Vector2 direction = (target.position - transform.position).normalized;

//             // Di chuyển quái vật
//             rb.linearVelocity = direction * speed;

//             // Cập nhật animation
//             if (direction.magnitude > 0)
//             {
//                 anim.SetFloat("moveX", direction.x);
//                 anim.SetBool("isMoving", true);
//             }
//             else
//             {
//                 anim.SetBool("isMoving", false);
//             }
//         }
//         else
//         {
//             // Tìm kiếm GameObject Player nếu chưa được gán
//             if (GameObject.FindGameObjectWithTag("Player") != null)
//             {
//                 target = GameObject.FindGameObjectWithTag("Player").transform;
//             }
//             anim.SetBool("isMoving", false);
//         }
//     }

//     public void Die()
//     {
//         anim.SetBool("isDead", true);
//         rb.linearVelocity = Vector2.zero; //Dừng di chuyển
//         GetComponent<Collider2D>().enabled = false; //Tắt collider
//         this.enabled = false; //Tắt script này
//         Destroy(gameObject, 5f); //Xóa object sau 5s (tùy chỉnh thời gian)
//     }
// }