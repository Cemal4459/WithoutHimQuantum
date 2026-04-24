using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections; // Zamanlayıcı (Coroutine) kullanmak için bu kütüphane şart!

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    [Header("Post Processing Ayarları")]
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    [Header("Geçiş Ayarları")]
    public float fadeDuration = 1.5f; // Rengin ne kadar sürede açılacağı (Saniye)

    // Dünyanın başlangıç ve hedef doygunluğu
    private float currentSaturation = -100f;
    private float targetSaturation = -100f;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.saturation.value = currentSaturation;
        }
    }

    // Eşya bulunduğunda bu tetiklenecek
    public void RestoreColor()
    {
        // Yeni hedefi belirliyoruz (20 birim daha parlak)
        targetSaturation += 20f;
        targetSaturation = Mathf.Clamp(targetSaturation, -100f, 0f);

        // Yumuşak geçiş animasyonunu başlatıyoruz
        StartCoroutine(LerpColor(targetSaturation));
    }

    // Zamanla rengi açan Coroutine (Sihrin gerçekleştiği yer)
    private IEnumerator LerpColor(float targetValue)
    {
        float elapsedTime = 0f;
        float startValue = colorAdjustments.saturation.value;

        // fadeDuration (1.5 saniye) boyunca adım adım çalışacak döngü
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Başlangıç değerinden hedef değere belirlenen sürede yumuşak geçiş (Lerp)
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / fadeDuration);

            if (colorAdjustments != null)
            {
                colorAdjustments.saturation.value = newValue;
            }

            yield return null; // Bir sonraki kareyi (frame) bekle ve döngüye devam et
        }

        // Döngü bitince tam olarak hedef değere oturt (küsurat kalmasın)
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = targetValue;
        }

        Debug.Log("Renk yumuşakça açıldı! Yeni Doygunluk Hedefi: " + targetValue);
    }

    // --- T Tuşu ile Test Kısmı (Cemal işini bitirene kadar durabilir) ---
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RestoreColor();
        }
    }
}