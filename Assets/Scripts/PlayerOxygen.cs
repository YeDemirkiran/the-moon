using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOxygen : MonoBehaviour
{
    public float maxOxygen;
    public float currentOxygen { get; private set; }

    public UnityAction oxygenUseStart { get; set; }
    public UnityAction oxygenUseEnd { get; set; }

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
        if (Input.GetKey(KeyCode.Q))
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

                if (!oxygenInUse)
                {
                    oxygenInUse = true;

                    oxygenUseStart.Invoke();
                }
            }
            else
            {
                if (oxygenInUse)
                {
                    if (audioCoroutine == null)
                    {
                        audioCoroutine = SoundManager.FadeAudio(this, audioSource, audioFadeDuration);
                        oxygenInUse = false;

                        oxygenUseEnd.Invoke();
                    }
                }
                else if (!audioSource.isPlaying)
                {
                    audioSource.volume = 1f;
                    audioSource.PlayOneShot(oxygenDepletedAudio);
                }  
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (oxygenInUse)
            {
                oxygenInUse = false;

                if (audioCoroutine == null)
                {
                    audioCoroutine = SoundManager.FadeAudio(this, audioSource, audioFadeDuration);
                }

                oxygenUseEnd.Invoke();
            } 
        }
    }
}