using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygenUI : MonoBehaviour
{
    PlayerOxygen oxygen;

    [SerializeField] Slider slider;
    [SerializeField] Image overlay;
    [SerializeField] TMP_Text valueText;
    [SerializeField] float overlayAppearDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        oxygen = PlayerOxygen.Instance;

        oxygen.oxygenUseStart += () => Fade(1f, overlayAppearDuration);
        oxygen.oxygenUseEnd += () => Fade(0f, overlayAppearDuration);
    }

    // Update is called once per frame
    void Update()
    {
        float value = oxygen.currentOxygen / oxygen.maxOxygen;
        slider.value = value;

        float percent = value * 100f;
        valueText.text = $"%{percent.ToString("0")}";
    }

    public void Fade(float transparency, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOverlay(transparency, duration));
    }

    IEnumerator FadeOverlay(float transparency, float duration)
    {
        float lerp = 0f;

        Color startingColor = overlay.color;
        Color targetColor = startingColor;
        targetColor.a = transparency;

        while (lerp <= 1f) 
        {
            lerp += Time.deltaTime / duration;

            overlay.color = Color.Lerp(startingColor, targetColor, lerp);

            yield return null;
        }
    }
}