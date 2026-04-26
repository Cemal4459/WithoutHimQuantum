using UnityEngine;

public class ChildItemCollectible : MonoBehaviour
{
    public AudioClip collectSound;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;

        if (ColorManager.instance != null)
        {
            ColorManager.instance.RestoreColor();
        }

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        gameObject.SetActive(false);
    }
}