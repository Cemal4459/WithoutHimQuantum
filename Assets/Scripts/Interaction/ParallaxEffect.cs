using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Header("Kamera Ayarı")]
    public Transform cameraTransform; // Ana Kamerayı buraya sürükleyeceğiz

    [Header("Derinlik Ayarı")]
    // Hız Çarpanı: 
    // 0 = Obje kamerayla aynı hızda gider (Karakterle beraber hareket eder gibi durur)
    // 1 = Obje hiç hareket etmez (Sonsuz uzaktaymış gibi durur, gökyüzü için ideal)
    [Range(0f, 1f)]
    public float parallaxFactorX; // Yatay paralaks hızı
    [Range(0f, 1f)]
    public float parallaxFactorY; // Dikey paralaks hızı (Opsiyonel, zıplarken derinlik hissi için)

    private Vector3 lastCameraPosition; // Kameranın bir önceki karedeki pozisyonu
    private float textureUnitSizeX; // (Opsiyonel: Sonsuz döngü için gerekli)

    void Start()
    {
        // Eğer kamera atanmadıysa otomatik olarak Ana Kamerayı bul
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Kameranın başlangıç pozisyonunu kaydet
        lastCameraPosition = cameraTransform.position;

        // (Opsiyonel) Sonsuz döngü için sprite boyutunu hesapla
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    // LateUpdate kullanıyoruz çünkü Kamera hareketini bitirdikten sonra arka planı oynatmalıyız
    void LateUpdate()
    {
        // Kameranın bu karede ne kadar hareket ettiğini hesapla
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Objeyi, kameranın hareketinin tersi yönünde, derinlik çarpanı kadar kaydır
        transform.position += new Vector3(deltaMovement.x * parallaxFactorX, deltaMovement.y * parallaxFactorY, 0);

        // Kameranın pozisyonunu güncelle
        lastCameraPosition = cameraTransform.position;

        // --- (Opsiyonel: Sonsuz Arka Plan Döngüsü) ---
        // Eğer harita çok uzunsa ve arka plan görseli bitiyorsa bu kısmı açabilirsin:
        /*
        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
        */
    }
}