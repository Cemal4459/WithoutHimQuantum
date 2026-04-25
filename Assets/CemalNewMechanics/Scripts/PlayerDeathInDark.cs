using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathInDark : MonoBehaviour
{
    public float deathDelay = 0.25f;

    private bool isInSafeZone;
    private float darkTimer;

    void Update()
    {
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

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}