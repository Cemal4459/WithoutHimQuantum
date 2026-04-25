using UnityEngine;
using System.Collections;

public class FallingTrap : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isTriggered = false;

    [Header("Tuzak Ayarları")]
    public float fallDelay = 0.3f; // Düşmeden önce kaç saniye titresin?
    public float fallGravity = 4f; // Ne kadar hızlı çakılsın?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // İŞTE BAHSETTİĞİM TETİKLEYİCİ KISIM BURASI
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tetikleyiciye giren objenin Tag'i "Player" ise ve tuzak henüz düşmediyse
        if (other.CompareTag("Player") && !isTriggered)
        {
            Debug.Log("SENSÖR PLAYER'I GÖRDÜ! SARKIT DÜŞÜYOR!"); // <-- Test yazımız
            isTriggered = true;
            StartCoroutine(ShakeAndDrop());
        }
    }

    private IEnumerator ShakeAndDrop()
    {
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;

        // Düşmeden önce sinsi bir şekilde titreme efekti
        while (elapsedTime < fallDelay)
        {
            // Sarkıtı çok hafif sağa sola titret
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * 0.05f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Titreme bitince tam yerine sabitle ve yerçekimini serbest bırak!
        transform.position = originalPos;
        rb.gravityScale = fallGravity;
    }

    // Yere çarptığında olacaklar
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eğer çarptığı şey zemin ise
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Yerde yuvarlanmasın diye fiziği dondur (Taş gibi saplansın)
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}