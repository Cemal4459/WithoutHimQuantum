using UnityEngine;

public class PlayerColorMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float darkMoveSpeed = 2f;
    public float jumpForce = 8f;

    public Transform groundCheck;
    public float checkRadius = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isInColorArea;
    private bool isGrounded;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = isInColorArea ? moveSpeed : darkMoveSpeed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    public void SetColorAreaState(bool state)
    {
        isInColorArea = state;
    }
}