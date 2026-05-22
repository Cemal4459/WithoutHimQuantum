using UnityEngine;
using UnityEngine.SceneManagement; // Sahneler arası geçiş ve yenileme için şart

public class MenuManager : MonoBehaviour
{
    // Restart Butonuna basıldığında bu fonksiyon çalışacak
    public void RestartGame()
    {
        Time.timeScale = 1f; // Zamanı tekrar normal hızına alıyoruz (Önemli!)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Mevcut sahneyi yeniden yükle
    }

    // Main Menu Butonuna basıldığında bu fonksiyon çalışacak
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Zamanı normale döndür
        
        // Buraya ana menü sahnenin tam adını yazmalısın. Örn: "MainMenu"
        // File -> Build Settings kısmında bu sahnenin ekli olduğundan emin ol!
        SceneManager.LoadScene("MainMenu"); 
    }
}