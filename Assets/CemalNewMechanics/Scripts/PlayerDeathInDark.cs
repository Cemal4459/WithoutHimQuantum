using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathInDark : MonoBehaviour
{
    public float deathDelay = 0.25f;

    private bool isInSafeZone;
    private float darkTimer;

    public bool darkDeathActive = false;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}