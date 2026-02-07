using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Child objeden Animator ve SpriteRenderer al
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveInput = GetHorizontalInput();
        ApplyMovement(moveInput);
        UpdateAnimation(moveInput);
        FlipSprite(moveInput);
    }

    private float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    private void ApplyMovement(float moveInput)
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void UpdateAnimation(float moveInput)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }
    }

    private void FlipSprite(float moveInput)
    {
        if (spriteRenderer != null && moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0;
        }
    }
}