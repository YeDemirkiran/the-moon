using UnityEngine;

public class Groundbug : MonoBehaviour
{
    [SerializeField] Animator animator;

    bool canJump = false, jumped = false;

    PlayerController player;

    private void Start()
    {
        player = PlayerController.instance;
    }

    // Update is called once per frame
    void Update()
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
                    player.RotateTowards(transform.position, 0.25f);
                }
                else if (player.rotationComplete)
                {
                    jumped = true;
                    animator.SetTrigger("jumpscare");
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