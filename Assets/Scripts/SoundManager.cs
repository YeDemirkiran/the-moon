using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    float audioMixerOriginalVolume, audioMixerOriginalPitch;

    IEnumerator Start()
    {
        audioMixer.GetFloat("sfxVolume", out audioMixerOriginalVolume);
        audioMixer.GetFloat("sfxPitch", out audioMixerOriginalPitch);

        while (GameManager.instance == null) yield return null;
        GameManager gameManager = GameManager.instance;

        gameManager.eventsAtPause += () => audioMixer.SetFloat("sfxVolume", -80f);
        gameManager.eventsAtPause += () => audioMixer.SetFloat("sfxPitch", 1f);

        gameManager.eventsAtResume += () => audioMixer.SetFloat("sfxVolume", audioMixerOriginalVolume);
        gameManager.eventsAtResume += () => audioMixer.SetFloat("sfxPitch", audioMixerOriginalPitch);
    }

    public static AudioSource CreateFollowing3DAudio(Transform caller, AudioClip clipToPlay, bool destroyWhenFinished = true, AudioMixerGroup group = null)
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

        return audioSource;
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