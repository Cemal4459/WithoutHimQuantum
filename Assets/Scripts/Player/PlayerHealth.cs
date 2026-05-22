using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Elements")]
    public GameObject[] heartImages; 
    public GameObject inGameMenuPanel; // Yeni ekledik: Açılacak olan menü paneli

    private PlayerMovement playerMovement;
    private bool isGameOver = false;

    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        
        // Oyun başında panelin kapalı olduğundan emin olalım
        if (inGameMenuPanel != null) inGameMenuPanel.SetActive(false);
        
        UpdateHeartUI();
    }

    void Update()
    {
        // Oyuncu ölmediyse ve ESC tuşuna basarsa menüyü aç/kapat
        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void TakeDamage()
    {
        if (isGameOver) return;

        currentHealth--; 
        UpdateHeartUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            if (playerMovement != null) playerMovement.Respawn();
        }
    }

    // ESC tuşuna basıldığında oyunu durdurup menüyü açan fonksiyon
    void ToggleMenu()
    {
        bool isActive = inGameMenuPanel.activeSelf;
        inGameMenuPanel.SetActive(!isActive);

        // Eğer menü açıldıysa zamanı durdur (0), kapandıysa normal hıza al (1)
        Time.timeScale = isActive ? 1f : 0f;
    }

    // Canlar tamamen bittiğinde çalışacak fonksiyon
    void GameOver()
    {
        isGameOver = true;
        inGameMenuPanel.SetActive(true); // Paneli aç
        Time.timeScale = 0f; // Oyunu dondur
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth) heartImages[i].SetActive(true);
            else heartImages[i].SetActive(false);
        }
    }
}