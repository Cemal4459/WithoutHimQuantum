using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    [Header("Post Processing")]
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    [Header("Geçiş")]
    public float restoreDuration = 1.5f;

    [Header("Renk Seviyeleri")]
    public float[] saturationLevels = { -100f, -85f, -65f, -35f, 0f };

    private int collectedCount;
    private Coroutine colorRoutine;
    [Header("Scene Start")]
public bool resetProgressOnThisScene = false;

    void Awake()
    {
        instance = this;
    }

void Start()
{
    if (resetProgressOnThisScene)
    {
        PlayerPrefs.DeleteKey("CollectedChildItems");
        PlayerPrefs.Save();
    }

    if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
    {
        collectedCount = PlayerPrefs.GetInt("CollectedChildItems", 0);
        collectedCount = Mathf.Clamp(collectedCount, 0, saturationLevels.Length - 1);

        colorAdjustments.saturation.value = saturationLevels[collectedCount];

        Debug.Log("Scene başlangıç renk seviyesi: " + collectedCount);
    }
}

    public void RestoreColor()
    {
        if (colorAdjustments == null) return;

        collectedCount++;
        collectedCount = Mathf.Clamp(collectedCount, 0, saturationLevels.Length - 1);

        PlayerPrefs.SetInt("CollectedChildItems", collectedCount);
        PlayerPrefs.Save();

        float targetSaturation = saturationLevels[collectedCount];

        if (colorRoutine != null)
            StopCoroutine(colorRoutine);

        colorRoutine = StartCoroutine(LerpColor(targetSaturation, restoreDuration));

        Debug.Log("Eşya toplandı. Yeni renk seviyesi: " + collectedCount);
    }

    private IEnumerator LerpColor(float targetValue, float duration)
    {
        float elapsed = 0f;
        float startValue = colorAdjustments.saturation.value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            colorAdjustments.saturation.value = Mathf.Lerp(startValue, targetValue, elapsed / duration);
            yield return null;
        }

        colorAdjustments.saturation.value = targetValue;
    }

    [ContextMenu("Reset Trailer Color Progress")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("CollectedChildItems");
        PlayerPrefs.Save();

        collectedCount = 0;

        if (colorAdjustments != null)
            colorAdjustments.saturation.value = saturationLevels[0];

        Debug.Log("Renk progress sıfırlandı.");
    }
}