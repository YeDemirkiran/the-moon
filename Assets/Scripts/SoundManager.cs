using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void CreateFollowing3DAudio(Transform caller, AudioClip clipToPlay, bool destroyWhenFinished = true, AudioMixerGroup group = null)
    {
        GameObject soundObject = new GameObject("AudioObject");
        soundObject.transform.position = caller.position;

        ParentFollower parentFollower = soundObject.AddComponent<ParentFollower>();
        parentFollower.parent = caller;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.outputAudioMixerGroup = group;
        audioSource.clip = clipToPlay;
        audioSource.Play();

        if (destroyWhenFinished)
        {
            parentFollower.StartCoroutine(DestroyOnClipEnd(audioSource));
        }
    }

    public static Coroutine FadeAudio(MonoBehaviour caller, AudioSource audioSource, float duration)
    {
        return caller.StartCoroutine(FadeAudio(audioSource, duration));
    }

    static IEnumerator DestroyOnClipEnd(AudioSource audioSource)
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);

        DestroyImmediate(audioSource.gameObject);
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