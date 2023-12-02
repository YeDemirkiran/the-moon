using UnityEngine;

public class Groundbug : MonoBehaviour
{
    [SerializeField] Animator animator;

    bool canJump = false, jumped = false;    

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
                jumped = true;
                animator.SetTrigger("jumpscare");
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