using UnityEngine;
using System.Collections; // Zamanlayıcı (Coroutine) için gerekli

public class Geyser : MonoBehaviour
{
    [Header("Görsel Ayarlar")]
    [SerializeField] private Sprite idleSprite;    // Doruk'un çizdiği suyun durgun hali
    [SerializeField] private Sprite activeSprite;  // Doruk'un çizdiği suyun fışkırdığı hali
    private SpriteRenderer spriteRenderer;

    [Header("Fizik Ayarları")]
    [SerializeField] private float boostForce = 15f; // Yukarı fırlatma gücü (Impulse)

    [Header("Zamanlama Ayarları (Saniye)")]
    [SerializeField] private float offTime = 3f;      // Ne kadar süre kapalı kalsın
    [SerializeField] private float warningTime = 1f;  // Fışkırmadan önce ne kadar süre titreşsin (isteğe bağlı)
    [SerializeField] private float activeDuration = 2f; // Ne kadar süre su fışkırsın

    private BoxCollider2D boostTrigger; // Karakteri algılayan Trigger
    private bool isGeyserActive = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boostTrigger = GetComponent<BoxCollider2D>();

        // Başlangıç durumu: Kapalı
        spriteRenderer.sprite = idleSprite;
        boostTrigger.enabled = false; // Kapalıyken boost atmasın

        // Gayzer döngüsünü başlat
        StartCoroutine(GeyserCycle());
    }

    // GAYZER DÖNGÜSÜ (Coroutine)
    IEnumerator GeyserCycle()
    {
        while (true) // Oyun açık olduğu sürece dönsün
        {
            // 1. AŞAMA: KAPALI (BEKLEME)
            isGeyserActive = false;
            spriteRenderer.sprite = idleSprite;
            boostTrigger.enabled = false; // Algılayıcı kapalı
            yield return new WaitForSeconds(offTime);

            // 2. AŞAMA: UYARI (İsteğe bağlı - Titreşim eklenebilir)
            // (Şimdilik boş bırakıyorum, sadece bekleme ekliyorum)
            yield return new WaitForSeconds(warningTime);

            // 3. AŞAMA: AKTİF (FIŞKIRMA)
            isGeyserActive = true;
            spriteRenderer.sprite = activeSprite; // FIŞKIRAN GÖRSELE GEÇ
            boostTrigger.enabled = true; // Algılayıcıyı AÇ (Boost atmaya hazır)
            yield return new WaitForSeconds(activeDuration);
        }
    }

    // KARAKTERİ YUKARI FIRLATMA
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sadece Gayzer AKTİFKEN ve Karakter (Player) çarptıysa çalışsın
        if (isGeyserActive && collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Tutarlılık için önce dikey hızı sıfırla (farklı yüksekliklerden düşerse aynı fırlasın)
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);

                // YUKARI DOĞRU ANİ GÜÇ UYGULA (Impulse)
                playerRb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse);

                // (İsteğe bağlı: Fırlarken bir ses efekti çalınabilir)
            }
        }
    }
}