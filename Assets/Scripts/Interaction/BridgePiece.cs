using UnityEngine;
using System.Collections;

public class BridgePiece : MonoBehaviour
{
    public float fallDelay = 0.25f;
    public float destroyDelay = 3f;

    private bool activated = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (activated) return;

        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
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

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}