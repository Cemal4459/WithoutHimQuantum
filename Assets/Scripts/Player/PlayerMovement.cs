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

    [Header("Wall Jump")]
    public Transform wallCheck;
    public float wallSlideSpeed = 2f;
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 9f;
    public float wallJumpTime = 0.2f;

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
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping;

    private bool isDashing;
    private bool canDash = true;

    public ParticleSystem runDust;
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
        CheckWall();

        if (!isDashing && !isWallJumping)
        {
            Move();
            Flip();
        }

        WallSlide();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallSliding)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                Jump();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isWallSliding)
        {
            StartCoroutine(Dash());
        }

        UpdateAnimations();
        HandleRunDust();
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

    void CheckWall()
    {
        if (wallCheck == null) return;

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

            // Karakter sağa bakıyorsa, WallCheck'i sağ tarafa al
            if (wallCheck != null)
            {
                wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, wallCheck.localPosition.z);
            }
        }
        else if (move < 0)
        {
            facingDirection = -1f;
            spriteRenderer.flipX = true;

            // Karakter sola bakıyorsa, WallCheck'i sol tarafa al (X eksenini eksi yap)
            if (wallCheck != null)
            {
                wallCheck.localPosition = new Vector3(-Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, wallCheck.localPosition.z);
            }
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

    void HandleRunDust()
    {
        if (runDust == null) return;

        bool shouldPlayDust = Mathf.Abs(move) > 0.1f && isGrounded && !isDashing;

        if (shouldPlayDust && !runDust.isPlaying)
            runDust.Play();

        else if (!shouldPlayDust && runDust.isPlaying)
            runDust.Stop();
    }
}