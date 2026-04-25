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

    [Header("Wall")]
    public float wallSlideSpeed = 2f;
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 9f;
    public float wallJumpTime = 0.2f;

    [Header("Checks")]
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float move;
    private float facingDirection = 1f;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping;

    private bool isDashing;
    private bool canDash = true;
    public Transform currentCheckpoint;

    void Start()
    {
        currentCheckpoint = transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();
            
    }

    public void Respawn()
    {
    transform.position = currentCheckpoint.position;
    GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        CheckSurroundings();

        if (!isDashing && !isWallJumping)
        {
            Move();
            Flip();
        }

        WallSlide();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallSliding)
                WallJump();
            else if (isGrounded)
                Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isWallSliding)
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

    void WallSlide()
    {
        if (isTouchingWall && !isGrounded && move != 0)
        {
            isWallSliding = true;

            if (rb.linearVelocity.y < -wallSlideSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    void WallJump()
    {
        isWallJumping = true;

        float wallJumpDirection = -facingDirection;

        rb.linearVelocity = new Vector2(
            wallJumpDirection * wallJumpForceX,
            wallJumpForceY
        );

        facingDirection = wallJumpDirection;
        spriteRenderer.flipX = facingDirection < 0;

        Invoke(nameof(StopWallJump), wallJumpTime);
    }

    void StopWallJump()
    {
        isWallJumping = false;
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
        );

        isTouchingWall = Physics2D.OverlapCircle(
            wallCheck.position,
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
        animator.SetBool("IsWallSliding", isWallSliding);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }
}