using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerDeathInDark player = other.GetComponent<PlayerDeathInDark>();

        if (player != null)
        {
            player.SetSafeZoneState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerDeathInDark player = other.GetComponent<PlayerDeathInDark>();

        if (player != null)
        {
            player.SetSafeZoneState(false);
        }
    }
}