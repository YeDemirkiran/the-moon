using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float minBPM, maxBPM;
    public float dangerBPM;

    float currentBPM;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip firstBeat, secondBeat;

    // Start is called before the first frame update
    void Awake()
    {
        currentBPM = minBPM;
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

                Debug.Log("1");
            }

            if (canSystole)
            {
                secondBeatTimer += Time.deltaTime;

                if (secondBeatTimer >= systoleInterval)
                {
                    canSystole = false;
                    secondBeatTimer = 0f;
                    audioSource.PlayOneShot(secondBeat);
                }
            }
            

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}