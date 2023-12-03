using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Groundbug : MonoBehaviour
{
    public Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpscareClip;

    bool canJump = false, jumped = false, canDestroy = false;

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
            if (!jumped)
            {
                Debug.Log("Jumped:");
                Debug.Log(jumped);

                if (!player.rotationStarted)
                {
                    player.RotateTowards(transform.position, 0.5f);
                }
                else if (player.rotationComplete)
                {
                    jumped = true;

                    StartCoroutine(Jumpscare());
                }
            }
        }
    }

    IEnumerator Jumpscare()
    {
        yield return new WaitForSeconds(0.25f);

        

        animator.SetTrigger("jumpscare");

        SoundManager.CreateFollowing3DAudio(transform, jumpscareClip, true, audioSource.outputAudioMixerGroup);

        if (!player.audioSource.isPlaying)
        {
            player.audioSource.PlayOneShot(player.scaredClip, 2f);
        }

        player.fpsController.canRotate = false;
        float preYaw = player.fpsController.yaw;

        for (float timer = 0f; timer < 1f; timer += Time.deltaTime)
        {
            player.fpsController.yaw += 1000f * Time.deltaTime;
            yield return null;
        }

        player.fpsController.yaw = preYaw;
        player.fpsController.canRotate = true;

        canDestroy = true;

        Debug.Log("Jumpscare Done");
        Destroy(gameObject);
    }
}