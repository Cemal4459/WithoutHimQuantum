using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yenilemek için

public class FallingTrap : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFalling = false;
    private bool hasLanded = false;

    [SerializeField] float fallGravity = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Başlangıçta tag "Untagged" veya "Enemy" olabilir
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
        // 1. OYUNCUYA ÇARPMA DURUMU (Ölüm)
        if (collision.gameObject.CompareTag("Player") && !hasLanded)
        {
            // Buraya kendi Checkpoint sistemini yazabilirsin. 
            // Şimdilik en basitinden sahneyi baştan başlatıyoruz:
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        // 2. YERE ÇARPMA DURUMU (Zemine Dönüşme)
        if (collision.gameObject.CompareTag("Ground") && !hasLanded)
        {
            hasLanded = true;
            rb.bodyType = RigidbodyType2D.Static; // Hareketini dondur

            // KODLA TAG VE LAYER DEĞİŞTİRME
            gameObject.tag = "Ground";
            gameObject.layer = LayerMask.NameToLayer("Ground");

            // Not: Unity'de "Ground" isminde bir Layer oluşturduğundan emin ol kanka.
        }
    }
}