using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    [Header("Post Processing Ayarları")]
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    [Header("Geçiş Ayarları")]
    public float restoreDuration = 1.5f; // Eşya bulunca renklenme süresi
    public float lostDuration = 1.0f;    // Girişte rengin yavaşça solma süresi (Daha uzun/sinematik)

    [Header("Renk Açılma Seviyeleri")]
    // Hedefler: -95, -85, -65, -35, 0 (Senin harika oranların)
    public float[] saturationLevels = { -95f, -85f, -65f, -35f, 0f };
    private int currentStep = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            // OYUN RENKLİ BAŞLIYOR!
            colorAdjustments.saturation.value = 0f;
        }

        // --- Game Jam Hızıyla Test İçin ---
        // Oyun başladıktan 2 saniye sonra otomatik solmayı başlatıyoruz.
        // Gerçek oyunda bunu çocuğun kaybolduğu tetikleyici (Trigger) anına bağlamalısınız.
        Invoke("StartDesaturationSequence", 1.0f);
    }

    // Bu methodu çocuğun kaybedildiği an (Cutscene bittiğinde veya Trigger'a değdiğinde) çağırmalısınız.
    public void StartDesaturationSequence()
    {
        if (colorAdjustments != null)
        {
            Debug.Log("Umut kayboluyor... Dünya grileşiyor.");
            // Hedefimiz zifiri siyah-beyaz (-100), Süremiz sinematik (4 saniye)
            StartCoroutine(LerpColor(-100f, lostDuration));
        }
    }

    // Eşya bulunduğunda bu tetiklenecek (Hızlı açılma)
    public void RestoreColor()
    {
        if (currentStep < saturationLevels.Length)
        {
            float targetSaturation = saturationLevels[currentStep];
            // Hedefimiz Array'deki değer, Süremiz hızlı (1.5 saniye)
            StartCoroutine(LerpColor(targetSaturation, restoreDuration));
            currentStep++;
        }
    }

    // Artık bu tek zamanlayıcı hem solma hem açılma için kullanılıyor!
    private IEnumerator LerpColor(float targetValue, float duration)
    {
        float elapsedTime = 0f;
        float startValue = colorAdjustments.saturation.value;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);

            if (colorAdjustments != null)
            {
                colorAdjustments.saturation.value = newValue;
            }

            yield return null;
        }

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = targetValue;
        }

        Debug.Log($"Renk geçişi tamamlandı! Yeni Doygunluk: {targetValue}");
    }

    // --- T Tuşu ile Test Kısmı ---
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RestoreColor();
        }
    }
}