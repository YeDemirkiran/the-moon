using System.Collections;
using UnityEngine;

public enum Cycle { First, Second }

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Heartbeat")]
    public float minBPM;
    public float maxBPM, dangerBPM;    
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip firstBeat, secondBeat;

    public float currentBPM { get; private set; }
    public Cycle cycle { get; private set; }

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
}