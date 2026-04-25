using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Dash")]
    public float dashSpeed = 14f;
    public float dashTime = 0.15f;
    public float dashCooldown = 0.6f;

    [Header("Checks")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trail;

    private float move;
    private float facingDirection = 1f;

    private bool isGrounded;
    private bool isDashing;
    private bool canDash = true;

    public Transform currentCheckpoint;
    
    void Start()
    {
        currentCheckpoint = transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();

        if (trail != null)
            trail.emitting = false;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint.position;
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        CheckGround();

        if (!isDashing)
        {
            Move();
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        UpdateAnimations();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        if (trail != null)
            trail.emitting = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        if (trail != null)
            trail.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
        );
    }

    void Flip()
    {
        if (move > 0)
        {
            facingDirection = 1f;
            spriteRenderer.flipX = false;
        }
        else if (move < 0)
        {
            facingDirection = -1f;
            spriteRenderer.flipX = true;
        }
    }

    void UpdateAnimations()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", Mathf.Abs(move));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsDashing", isDashing);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}