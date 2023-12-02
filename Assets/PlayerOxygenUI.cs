using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygenUI : MonoBehaviour
{
    PlayerOxygen oxygen;

    [SerializeField] Slider slider;
    [SerializeField] Image overlay;
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
        slider.value = oxygen.currentOxygen / oxygen.maxOxygen;
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