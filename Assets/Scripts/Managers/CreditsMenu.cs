using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CreditsMenu : MonoBehaviour
{
    public Image fadePanel;
    public float fadeSpeed = 2f;
    private bool isTransitioning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
        {
            StartCoroutine(FadeAndLoadMenu());
        }
    }

    IEnumerator FadeAndLoadMenu()
    {
        isTransitioning = true;

        Color color = fadePanel.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            fadePanel.color = color;
            yield return null;
        }

        SceneManager.LoadScene("MainMenu");
    }
}