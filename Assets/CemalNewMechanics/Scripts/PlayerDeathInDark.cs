using UnityEngine;

public class PlayerDeathInDark : MonoBehaviour
{
    public float deathDelay = 0.25f;

    private bool isInSafeZone;
    private float darkTimer;

    public bool darkDeathActive = false;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    public MirrorLeverController mirrorLeverController;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!darkDeathActive)
        {
            darkTimer = 0f;
            return;
        }

        if (isInSafeZone)
        {
            darkTimer = 0f;
            return;
        }

        darkTimer += Time.deltaTime;

        if (darkTimer >= deathDelay)
        {
            Die();
        }
    }

    public void SetSafeZoneState(bool state)
    {
        isInSafeZone = state;
    }

    public void SetDarkDeathActive(bool state)
    {
        darkDeathActive = state;
        darkTimer = 0f;
    }

    void Die()
{
    if (playerMovement != null && playerMovement.currentCheckpoint != null)
    {
        transform.position = playerMovement.currentCheckpoint.position;
    }

    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    if (mirrorLeverController != null)
    {
        mirrorLeverController.ResetLeverToStart();
    }

    darkTimer = 0f;
    isInSafeZone = false;
}
}