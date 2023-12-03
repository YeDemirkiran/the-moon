using System.Collections;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public static bool currentlyJumpscaring = false;

    public Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpscareClip;

    bool canJump = false, jumped = false;

    PlayerController player;

    private void Start()
    {
        player = PlayerController.instance;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!canJump)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                canJump = true;
            }
        }
        else
        {
            if (!jumped && !currentlyJumpscaring)
            {
                currentlyJumpscaring = true;

                Debug.Log("Creature name: " + gameObject.name);
                Debug.Log("Jumped:");
                Debug.Log(jumped);

                StartCoroutine(Jumpscare());

                
            }
        }
    }

    IEnumerator Jumpscare()
    {
        while (player.rotationStarted) yield return null;

        player.RotateTowards(transform.position, 0.5f);

        while (!player.rotationComplete) yield return null;

        player.rotationStarted = false;
        jumped = true;

        yield return new WaitForSeconds(0.25f);

        animator.SetTrigger("jumpscare");

        yield return null;
        SoundManager.CreateFollowing3DAudio(transform, jumpscareClip, true, audioSource.outputAudioMixerGroup);
        player.audioSource.PlayOneShot(player.scaredClip, 2f);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        player.fpsController.canRotate = false;
        float preYaw = player.fpsController.yaw;

        for (float timer = 0f; timer < 1f; timer += Time.deltaTime)
        {
            player.fpsController.yaw += 1000f * Time.deltaTime;
            yield return null;
        }

        player.fpsController.yaw = preYaw;
        player.fpsController.canRotate = true;

        Debug.Log("Jumpscare Done");
        Destroy(gameObject);

        currentlyJumpscaring = false;
        
    }
}