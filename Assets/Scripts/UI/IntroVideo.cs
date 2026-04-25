using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "ForestScene";

    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadNextScene();
        }
    }

    void EndReached(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}