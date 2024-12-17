using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    float horizontal;
    float vertical;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x += (3f * horizontal * Time.deltaTime * moveSpeed);
        position.y += (3f * vertical * Time.deltaTime * moveSpeed);
        rb.MovePosition(position);
    }


}