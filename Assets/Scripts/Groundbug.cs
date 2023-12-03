using System.Threading.Tasks;
using UnityEngine;

public class Groundbug : MonoBehaviour
{
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
    public async void Update()
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
                if (!player.rotationStarted)
                {
                    player.RotateTowards(transform.position, 0.5f);
                }
                else if (player.rotationComplete)
                {
                    await Task.Delay(250);

                    jumped = true;
                    animator.SetTrigger("jumpscare");

                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(jumpscareClip);
                    }

                    if (!player.audioSource.isPlaying)
                    {
                        player.audioSource.PlayOneShot(player.scaredClip, 2f);
                    }

                    player.fpsController.canRotate = false;
                    float preYaw = player.fpsController.yaw;

                    for (float timer = 0f; timer < 1f; timer += Time.deltaTime)
                    { 
                        player.fpsController.yaw += 1000f * Time.deltaTime;
                        await Task.Yield();
                    }

                    player.fpsController.yaw = preYaw;
                    player.fpsController.canRotate = true;
                }
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    Debug.Log("Jumpscare Done");
                    Destroy(gameObject);
                }
            }
        }
    }
}