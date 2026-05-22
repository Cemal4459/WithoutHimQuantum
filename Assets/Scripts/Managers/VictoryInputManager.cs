using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Image bileşenini kontrol etmek için gerekli
using System.Collections; // Coroutine (zaman ayarlı fonksiyon) için gerekli

public class VictoryInputManager : MonoBehaviour
{
    [Header("Fade Ayarları")]
    public Image fadePanel; // Eklediğimiz siyah FadePanel objesi
    public float fadeDuration = 1.5f; // Ekranın kararma süresi (saniye cinsinden)

    private bool isFading = false; // Kararma işleminin birden çok kez tetiklenmesini önler

    void Start()
    {
        // Oyun başladığında panelin tamamen saydam olduğundan emin olalım
        if (fadePanel != null)
        {
            Color startColor = fadePanel.color;
            startColor.a = 0f;
            fadePanel.color = startColor;
            
            fadePanel.gameObject.SetActive(true); // Görünmez olsa da obje aktif olmalı
        }
    }

    void Update()
    {
        // Eğer zaten kararma başladıysa bir daha tıklamayı algılama
        if (isFading) return;

        // Farenin sol tuşuna (0) veya ESC tuşuna basıldığında kararma işlemini başlat
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FadeAndLoad());
        }
    }

    // Kararma işlemini ve sahne geçişini yöneten zamanlanmış fonksiyon
    IEnumerator FadeAndLoad()
    {
        isFading = true; // Kararma başladı olarak işaretle
        Time.timeScale = 1f; // Oyun donuksa normale çevir

        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;

        // Belirlenen süre boyunca Alpha değerini yavaşça artır
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime; // Geçen süreyi ekle
            
            // Alpha değerini 0 ile 1 arasında zamanla orantılı artırıyoruz
            panelColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = panelColor;
            
            yield return null; // Bir sonraki kareye (frame) kadar bekle
        }

        // Kararma tamamen bittiğinde diğer sahneyi yükle
        SceneManager.LoadScene("MainMenu");
    }
}