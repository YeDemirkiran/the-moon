using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static Coroutine FadeAudio(MonoBehaviour caller, AudioSource audioSource, float duration)
    {
        return caller.StartCoroutine(FadeAudio(audioSource, duration));
    }

    static IEnumerator FadeAudio(AudioSource audioSource, float fadeDuration)
    {
        float lerp = 0f;

        float startingVolume = audioSource.volume;

        while (lerp <= 1f)
        {
            lerp += Time.deltaTime / fadeDuration;

            audioSource.volume = Mathf.Lerp(startingVolume, 0f, lerp);

            yield return null;
        }

        audioSource.Stop();
    }
}