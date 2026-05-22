using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yüklemek için

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Elements")]
    public GameObject[] heartImages; // Hierarchy'deki 3 kalp objesini buraya atayacağız

    private PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = maxHealth;
        
        // Karakterin üzerindeki hareket koduna referans alıyoruz
        playerMovement = GetComponent<PlayerMovement>();
        
        UpdateHeartUI();
    }

    // DeathZone bu fonksiyonu çağıracak
    public void TakeDamage()
    {
        currentHealth--; // Canı 1 azalt
        UpdateHeartUI();

        if (currentHealth <= 0)
        {
            RestartGame();
        }
        else
        {
            // Eğer can varsa, PlayerMovement içindeki eski Respawn mantığını çalıştır
            if (playerMovement != null)
            {
                playerMovement.Respawn();
            }
            else
            {
                Debug.LogError("PlayerMovement script'i bulunamadı! Karakter ışınlanamıyor.");
            }
        }
    }

    // Kalp görsellerini ekranda açıp kapatan fonksiyon
    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].SetActive(true); // Can varsa göster
            }
            else
            {
                heartImages[i].SetActive(false); // Can gitmişse gizle
            }
        }
    }

    // Can tamamen bittiğinde sahneyi en baştan yükler
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}