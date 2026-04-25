using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Image fadePanel;
    public float fadeSpeed = 2f;

    public string gameSceneName = "ForestScene";
    public string creditsSceneName = "CreditsScene";

    public void StartGame()
    {
        StartCoroutine(FadeAndLoad(gameSceneName));
    }

    public void OpenCredits()
    {
        StartCoroutine(FadeAndLoad(creditsSceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        Color color = fadePanel.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            fadePanel.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}