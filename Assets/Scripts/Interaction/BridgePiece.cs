using UnityEngine;
using System.Collections;

public class BridgePiece : MonoBehaviour
{
    public float fallDelay = 0.25f;
    public float disappearDelay = 3f;

    private bool activated = false;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        ResetBridge();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (activated) return;

        if (collision.gameObject.GetComponentInParent<PlayerMovement>() != null)
        {
            activated = true;
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;

        yield return new WaitForSeconds(disappearDelay);

        col.enabled = false;
        sr.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;
    }
public void ResetBridge()
{
    StopAllCoroutines();

    activated = false;

    transform.position = startPosition;
    transform.rotation = startRotation;

    rb.bodyType = RigidbodyType2D.Dynamic;

    rb.linearVelocity = Vector2.zero;
    rb.angularVelocity = 0f;
    rb.gravityScale = 0f;

    rb.bodyType = RigidbodyType2D.Static;

    col.enabled = true;
    sr.enabled = true;
}
}