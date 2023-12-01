using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, parent.rotation, Time.deltaTime * rotationSpeed);
    }
}