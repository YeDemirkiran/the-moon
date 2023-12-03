using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerBreath : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<BreathSFX> breathSounds;

    float previousBPM;
    Vector2 previousBPMRange = new Vector2(0f, 0f);

    private void Awake()
    {
        breathSounds.Sort((x, y) => y.BPMrange.y.CompareTo(x.BPMrange.y));
    }

    // Update is called once per frame
    IEnumerator Start()
    {
        while (true)
        {
            if (previousBPM != PlayerHealth.Instance.currentBPM)
            {
                previousBPM = PlayerHealth.Instance.currentBPM;

                if (previousBPMRange.x >= previousBPM || previousBPMRange.y <= previousBPM)
                {
                    foreach (BreathSFX sfx in breathSounds)
                    {
                        if (previousBPM >= sfx.BPMrange.x && previousBPM <= sfx.BPMrange.y)
                        {
                            previousBPMRange = sfx.BPMrange;

                            float volume = audioSource.volume;
                            audioSource.loop = false;
                            bool startedFade = false;

                            while (audioSource.isPlaying)
                            {
                                if (!startedFade)
                                {
                                    startedFade = true;
                                    SoundManager.FadeAudio(this, audioSource, 1f);
                                }

                                yield return null;
                            }

                            audioSource.volume = volume;
                            audioSource.loop = true;
                            audioSource.clip = sfx.clips[Random.Range(0, sfx.clips.Length)];
                            audioSource.Play();
                            break;
                        }
                    }
                }
            }

            yield return null;
        }
    }
}

[System.Serializable]
public struct BreathSFX
{
    public Vector2 BPMrange;
    public AudioClip[] clips;
}