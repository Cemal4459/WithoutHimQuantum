using UnityEngine;
using System.Collections;

public class BiomeTrigger : MonoBehaviour
{
    [Header("Biyom Ayarları")]
    public Color targetParticleColor = Color.gray; // Bu bölgede partiküller ne renk olacak?
    public float transitionSpeed = 2.0f; // Renk kaç saniyede yumuşakça değişsin?

    private ParticleSystem camParticles;

    // Geçişin çakışmaması için hafızada tutuyoruz
    private static Coroutine activeColorCoroutine;
    private static BiomeTrigger activeTrigger;

    void Start()
    {
        // Oyun başlarken kameranın içindeki Particle System'i bul
        if (Camera.main != null)
        {
            camParticles = Camera.main.GetComponentInChildren<ParticleSystem>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer içeri giren objenin Tag'i "Player" ise
        if (other.CompareTag("Player") && camParticles != null)
        {
            // Eğer aynı biyoma tekrar girdiyse boşuna kodu yorma
            if (activeTrigger == this) return;
            activeTrigger = this;

            // Eğer çalışan başka bir renk geçişi varsa onu durdur ki renkler birbirine girmesin
            if (activeColorCoroutine != null)
            {
                StopCoroutine(activeColorCoroutine);
            }

            // Yeni renge yumuşak geçişi başlat
            activeColorCoroutine = StartCoroutine(SmoothColorTransition());
            Debug.Log("Yeni Biyoma Girildi! Partikül rengi yavaşça değişiyor...");
        }
    }

    private IEnumerator SmoothColorTransition()
    {
        var mainModule = camParticles.main;

        // Mevcut rengi al (startColor artık MinMaxGradient olduğu için color propertysini alıyoruz)
        Color startColor = mainModule.startColor.color;
        float elapsedTime = 0f;

        while (elapsedTime < transitionSpeed)
        {
            elapsedTime += Time.deltaTime;

            // Rengi başlangıçtan hedefe doğru yavaşça (Lerp ile) kaydır
            mainModule.startColor = Color.Lerp(startColor, targetParticleColor, elapsedTime / transitionSpeed);

            yield return null; // Bir sonraki kareyi (frame) bekle
        }

        // Süre bitince tam olarak hedef renge sabitle
        mainModule.startColor = targetParticleColor;
    }
}