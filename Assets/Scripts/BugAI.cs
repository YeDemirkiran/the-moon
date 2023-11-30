using UnityEngine;

public class BugAI : MonoBehaviour
{
    [SerializeField] float force;

    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * force, ForceMode.VelocityChange);
    }
}