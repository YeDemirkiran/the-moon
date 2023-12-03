using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
    async void Update()
    {
        if (previousBPM != PlayerHealth.Instance.currentBPM)
        {
            Debug.Log("Previous BPM != current BPM");

            previousBPM = PlayerHealth.Instance.currentBPM;

            if (previousBPMRange.x >= previousBPM || previousBPMRange.y <= previousBPM)
            {
                Debug.Log("Previous BPM range is outdated");

                foreach (BreathSFX sfx in breathSounds)
                {
                    if (previousBPM >= sfx.BPMrange.x && previousBPM <= sfx.BPMrange.y)
                    {

                        previousBPMRange = sfx.BPMrange;
                        Debug.Log("New BPM range: " + previousBPMRange);

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

                            await Task.Yield();
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
    }
}

[System.Serializable]
public struct BreathSFX
{
    public Vector2 BPMrange;
    public AudioClip[] clips;
}