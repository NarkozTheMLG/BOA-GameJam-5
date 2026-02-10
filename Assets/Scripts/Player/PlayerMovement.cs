using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] public bool isMovementEnabled = true;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

        if (!isMovementEnabled)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            UpdateAnimation(0);
            return;
        }
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
    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    public void DisableMovement()
    {
        isMovementEnabled = false;
    }

    public void SetMovement(bool enabled)
    {
        isMovementEnabled = enabled;
    }
}