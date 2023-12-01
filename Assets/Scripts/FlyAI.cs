using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlyAI : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 target;

    [SerializeField] float raycastDistance;
    [SerializeField] LayerMask raycastMask;
    [SerializeField] float upperSpeed = 10f;

    bool canFly = true;

    // Start is called before the first frame update
    void Start()
    {
        Fly(target);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.BoxCast(transform.position, Vector3.one / 2f, transform.forward, Quaternion.identity, raycastDistance, raycastMask))
        {
            canFly = false;
            transform.position += Vector3.up * upperSpeed * Time.deltaTime;
        }
        else
        {
            canFly = true;
        }
    }

    void Fly(Vector3 target)
    {
        StopAllCoroutines();
        StartCoroutine(FlyIE(target));
    }

    IEnumerator FlyIE(Vector3 target)
    {
        float lerp = 0f;

        float distance = Vector3.Distance(transform.position, target);

        while (lerp < 1f)
        {
            if (canFly)
            {
                lerp += speed * Time.deltaTime / distance;

                transform.position = Vector3.Slerp(transform.position, target, lerp);
            }

            yield return null;
        }
    }
}