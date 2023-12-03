using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] TMP_Text bpmText;

    [Header("Heart Animation")]
    [SerializeField] Image heartIcon;    
    [SerializeField] Vector3 normalScale, maxScale;
    [SerializeField] Color normalColor, dangerColor;

    [Header("Vignette Overlay")]
    Vignette vignette;
    [SerializeField] Volume volume;
    [SerializeField] float restIntensity, maxIntensity;
    [SerializeField] Color vignetteRestColor, vignetteDangerColor;

    PlayerHealth health;
    Cycle previousCycle = default;

    bool canUpdate = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerHealth.Instance != null);
        health = PlayerHealth.Instance;

        //yield return new WaitUntil(() => volume.HasInstantiatedProfile());
            
        volume.profile.TryGet(out vignette);

        canUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused) return;

        if (canUpdate)
        {
            UpdateValue();
            UpdateValueColor();

            UpdateHeartColor();
            UpdateHeartCycle();
        }
    }

    void UpdateValue()
    {
        bpmText.text = health.currentBPM.ToString("0");
    }

    void UpdateValueColor()
    {
        float bpm = health.currentBPM - health.dangerBPM;
        float threshold = health.maxBPM - health.dangerBPM;

        if (bpm >= 0f)
        {
            bpmText.color = Color.Lerp(normalColor, dangerColor, bpm / threshold);
        }
        else
        {
            bpmText.color = normalColor;
        }
    }

    void UpdateHeartColor()
    {
        float bpm = health.currentBPM - health.dangerBPM;
        float threshold =  health.maxBPM - health.dangerBPM;

        if (bpm >= 0f)
        {
            heartIcon.color = Color.Lerp(normalColor, dangerColor, bpm / threshold);
        }
        else
        {
            heartIcon.color = normalColor;
        }
    }

    void UpdateHeartCycle()
    {
        if (previousCycle != health.cycle)
        {
            StopAllCoroutines();
            heartIcon.transform.localScale = normalScale;

            previousCycle = health.cycle;

            switch (previousCycle)
            {
                case Cycle.First:
                    StartCoroutine(Beat(maxScale, maxIntensity, 0.05f));
                    break;

                case Cycle.Second:

                    StartCoroutine(Beat(Vector3.Lerp(normalScale, maxScale, 0.3f), Mathf.Lerp(restIntensity, maxIntensity, 0.3f), 0.05f));
                    break;
            }
        }
    }

    IEnumerator Beat(Vector3 scale, float vignetteIntensity, float duration)
    {
        float lerp = 0f;

        Vector3 startingScale = heartIcon.transform.localScale;
        Vector3 targetScale = scale;

        float startingIntensity = vignette.intensity.value;
        float targetIntensity = vignetteIntensity;

        Color startingColor = vignette.color.value;
        Color targetColor = vignetteDangerColor;

        float bpm = health.currentBPM - health.dangerBPM;
        float dangerCoefficient = bpm / (health.maxBPM - health.dangerBPM);

        while (lerp <= 1f)
        {
            lerp += Time.deltaTime / (duration / 2f);

            heartIcon.transform.localScale = Vector3.Lerp(startingScale, targetScale, lerp);

            if (bpm > 0f)
            {
                vignette.color.value = Color.Lerp(startingColor, targetColor, lerp);
                vignette.intensity.value = Mathf.Lerp(startingIntensity, targetIntensity * dangerCoefficient, lerp);
            }

            yield return null;
        }

        lerp = 0f;

        targetScale = startingScale;
        startingScale = heartIcon.transform.localScale;

        targetIntensity = startingIntensity;
        startingIntensity = vignette.intensity.value;

        targetColor = startingColor;
        startingColor = vignette.color.value;

        while (lerp <= 1f)
        {
            lerp += Time.deltaTime / (duration * 2f);

            heartIcon.transform.localScale = Vector3.Lerp(startingScale, targetScale, lerp);

            if (bpm > 0f)
            {
                vignette.color.value = Color.Lerp(startingColor, targetColor, lerp);
                vignette.intensity.value = Mathf.Lerp(startingIntensity, targetIntensity, lerp);
            }

            yield return null;
        }
    }
}