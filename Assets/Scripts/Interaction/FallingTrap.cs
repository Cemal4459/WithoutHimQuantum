using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFalling = false;
    private bool hasLanded = false;

    [SerializeField] float fallGravity = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            rb.gravityScale = fallGravity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Oyuncuya çarparsa checkpoint'e döndür
        if (collision.gameObject.CompareTag("Player") && !hasLanded)
        {
            PlayerMovement player = collision.gameObject.GetComponentInParent<PlayerMovement>();

            if (player != null)
            {
                player.Respawn();
            }

            return;
        }

        // Yere çarparsa zemin gibi davran
        if (collision.gameObject.CompareTag("Ground") && !hasLanded)
        {
            hasLanded = true;
            rb.bodyType = RigidbodyType2D.Static;

            gameObject.tag = "Ground";
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
    }
}