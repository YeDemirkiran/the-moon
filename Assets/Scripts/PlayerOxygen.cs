using System.Collections;
using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    public float maxOxygen;
    public float currentOxygen { get; private set; }
    [SerializeField] float usePercentagePerSecond = 5f;
    [SerializeField] float bpmDecreasePerPercent = 1.2f;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip oxygenDepletedAudio;
    [SerializeField] float audioFadeDuration = 0.25f;

    Coroutine audioCoroutine = null;

    public static PlayerOxygen Instance;
    PlayerHealth health;

    bool oxygenInUse = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health = PlayerHealth.Instance;
        currentOxygen = maxOxygen;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (currentOxygen >= 0f)
            {
                float percent = maxOxygen * (usePercentagePerSecond / 100f);
                currentOxygen -= percent * Time.deltaTime;

                health.currentBPM -= (bpmDecreasePerPercent * usePercentagePerSecond) * Time.deltaTime;

                if (!audioSource.isPlaying)
                {
                    if (audioCoroutine != null)
                    {
                        StopCoroutine(audioCoroutine);
                        audioCoroutine = null;

                        audioSource.volume = 1f;
                    }

                    audioSource.Play();
                }

                oxygenInUse = true;
            }
            else
            {
                if (oxygenInUse)
                {
                    if (audioCoroutine == null)
                    {
                        audioCoroutine = StartCoroutine(AudioFade());
                        oxygenInUse = false;
                    }
                }
                else if (!audioSource.isPlaying)
                {
                    audioSource.volume = 1f;
                    audioSource.PlayOneShot(oxygenDepletedAudio);
                }  
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (audioCoroutine == null)
            {
                audioCoroutine = StartCoroutine(AudioFade());
            }
        }
    }

    IEnumerator AudioFade()
    {
        float lerp = 0f;

        float startingVolume = audioSource.volume;

        while (lerp <= 1f) 
        {
            lerp += Time.deltaTime / audioFadeDuration;

            audioSource.volume = Mathf.Lerp(startingVolume, 0f, lerp);

            yield return null;   
        }

        audioSource.Stop();
    }
}