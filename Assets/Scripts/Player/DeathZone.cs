using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug logların aynen kalabilir, hatayı çözmede yardımcı olur
        Debug.Log("DeathZone'a giren obje: " + collision.name);

        // Child objeden tetiklendiği için parent objedeki PlayerHealth script'ini arıyoruz
        PlayerHealth playerHealth = collision.GetComponentInParent<PlayerHealth>();
        
        Debug.Log("Parent'ta PlayerHealth var mı: " + (playerHealth != null));

        if (playerHealth != null)
        {
            // Karakteri doğrudan ışınlamak yerine önce hasar sistemini çalıştırıyoruz
            playerHealth.TakeDamage();
            Debug.Log("PlayerHealth tetiklendi, hasar verildi.");
        }
        else
        {
            Debug.LogWarning("DeathZone'a bir şey girdi ama üzerinde PlayerHealth bulunamadı!");
        }
    }
}