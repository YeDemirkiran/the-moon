using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Heart Animation")]
    [SerializeField] Image heartIcon;
    [SerializeField] Vector3 normalScale, maxScale;
    [SerializeField] Color normalColor, dangerColor;

    PlayerHealth health;
    Cycle previousCycle = default;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerHealth.Instance != null);
        health = PlayerHealth.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeartColor();
        UpdateHeartCycle();
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
                    StartCoroutine(Beat(maxScale, 0.05f));
                    break;

                case Cycle.Second:

                    StartCoroutine(Beat(Vector3.Lerp(normalScale, maxScale, 0.3f), 0.05f));
                    break;
            }
        }
    }

    IEnumerator Beat(Vector3 scale, float duration)
    {
        float lerp = 0f;

        Vector3 startingScale = heartIcon.transform.localScale;
        Vector3 targetScale = scale;

        while (lerp <= 1f)
        {
            lerp += Time.deltaTime / (duration / 2f);

            heartIcon.transform.localScale = Vector3.Lerp(startingScale, targetScale, lerp);

            yield return null;
        }

        lerp = 0f;
        targetScale = startingScale;
        startingScale = heartIcon.transform.localScale;

        while (lerp <= 1f)
        {
            lerp += Time.deltaTime / (duration * 2f);

            heartIcon.transform.localScale = Vector3.Lerp(startingScale, targetScale, lerp);

            yield return null;
        }
    }
}