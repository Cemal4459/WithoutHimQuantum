using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Image fadePanel;
    public float fadeSpeed = 2f;

    public void StartGame()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        Color color = fadePanel.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            fadePanel.color = color;
            yield return null;
        }

        SceneManager.LoadScene("ForestScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}