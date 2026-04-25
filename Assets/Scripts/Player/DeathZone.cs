using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("DeathZone'a giren obje: " + collision.name);
        Debug.Log("Tag: " + collision.tag);

        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        Debug.Log("Aynı objede PlayerMovement var mı: " + (player != null));

        PlayerMovement parentPlayer = collision.GetComponentInParent<PlayerMovement>();
        Debug.Log("Parent'ta PlayerMovement var mı: " + (parentPlayer != null));

        if (parentPlayer != null)
        {
            parentPlayer.Respawn();
            Debug.Log("Respawn çalıştı.");
        }
    }
}