using UnityEngine;

public class LightPuzzleZoneTrigger : MonoBehaviour
{
    [Header("Objects To Enable")]
    public GameObject colorRaySystem;
    public GameObject glassPrism;
    public GameObject leftRail;
    public GameObject rightRail;
    public GameObject leftHandle;
    public GameObject rightHandle;
    public GameObject safeZone;

    [Header("Darkness")]
    public GameObject darknessOverlay;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (activated) return;

        activated = true;

        if (darknessOverlay != null)
            darknessOverlay.SetActive(true);

        if (colorRaySystem != null)
            colorRaySystem.SetActive(true);

        if (glassPrism != null)
            glassPrism.SetActive(true);

        if (leftRail != null)
            leftRail.SetActive(true);

        if (rightRail != null)
            rightRail.SetActive(true);

        if (leftHandle != null)
            leftHandle.SetActive(true);

        if (rightHandle != null)
            rightHandle.SetActive(true);

        if (safeZone != null)
            safeZone.SetActive(true);
    }
}