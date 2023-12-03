using System.Collections;
using UnityEngine;

public enum Cycle { First, Second }

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [SerializeField] float bpmIncreasePerSecondInDark = 1f;

    [Header("Heartbeat")]
    public float minBPM;
    public float maxBPM, dangerBPM;    
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip firstBeat, secondBeat;
    [SerializeField] AnimationCurve bpmChangeCurve;

    public float currentBPM { get; set; }
    public Cycle cycle { get; private set; }

    Coroutine bpmRoutine = null;

    void Awake()
    {
        currentBPM = minBPM;

        Instance = this;
    }

    IEnumerator Start()
    {
        float firstBeatTimer = 1f, secondBeatTimer = 0f;
        bool canSystole = false;

        while (true)
        {
            if (!Flashlight.Instance.open)
            {
                currentBPM += bpmIncreasePerSecondInDark * Time.deltaTime;
            }

            currentBPM = Mathf.Clamp(currentBPM, minBPM, maxBPM);

            float beatInterval = 60f / currentBPM;
            float systoleInterval = beatInterval * 0.3f;

            firstBeatTimer += Time.deltaTime;

            if (firstBeatTimer >= beatInterval)
            {
                firstBeatTimer = 0f;
                audioSource.PlayOneShot(firstBeat);
                canSystole = true;

                cycle = Cycle.First;
            }

            if (canSystole)
            {
                secondBeatTimer += Time.deltaTime;

                if (secondBeatTimer >= systoleInterval)
                {
                    canSystole = false;
                    secondBeatTimer = 0f;
                    audioSource.PlayOneShot(secondBeat);

                    cycle = Cycle.Second;
                }
            }
            
            yield return null;
        }
    }

    public void SetBPM(float bpm, float increasePerSecond)
    {
        StopBPMChange();

        bpmRoutine = StartCoroutine(NewBPM(bpm, increasePerSecond));
    }

    public void StopBPMChange()
    {
        if (bpmRoutine != null)
        {
            StopCoroutine(bpmRoutine);
            bpmRoutine = null;
        }
    }

    IEnumerator NewBPM(float newBPM, float increasePerSecond)
    {
        float lerp = 0f;

        float startinBPM = currentBPM;

        float completionTime = (newBPM - startinBPM) / increasePerSecond;

        while (lerp < 1.1f)
        {
            lerp += Time.deltaTime / completionTime;
            currentBPM = Mathf.Lerp(startinBPM, newBPM, bpmChangeCurve.Evaluate(lerp));

            yield return null;
        }
    }
}