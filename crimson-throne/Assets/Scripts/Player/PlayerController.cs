using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 4.5f;
    Rigidbody2D rb2d;
    PlayerInputActions inputActions;
    Animator animator;
    Vector2 currentInput = Vector2.zero;
    float lookXDirection = 1f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovement;
        inputActions.Player.Move.canceled += OnMovement;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (currentInput.magnitude > 0.01f)
        {
            Vector2 position = rb2d.position;
            position += currentInput * speed * Time.deltaTime;
            rb2d.MovePosition(position);
        }

        Vector2 move = new Vector2(currentInput.x, currentInput.y);
        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookXDirection = Mathf.Sign(move.x);
        }

        animator.SetFloat("Look X", lookXDirection);
        animator.SetFloat("Speed", move.magnitude);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentInput = context.ReadValue<Vector2>();
        }
        if (context.canceled)
        {
            currentInput = Vector2.zero;
        }
    }
}
