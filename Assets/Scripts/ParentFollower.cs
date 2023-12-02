using UnityEngine;

public enum Axis { x, y, z, All}

public class ParentFollower : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] Axis axis;
    [SerializeField] Vector3 offset;
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.position + offset;

        Quaternion rotation;
        rotation = Quaternion.Slerp(transform.rotation, parent.rotation, Time.deltaTime * rotationSpeed);

        switch (axis)
        {
            case Axis.x:
                rotation.y = transform.rotation.y;
                rotation.z = transform.rotation.z;
                break;
            case Axis.y:
                rotation.x = transform.rotation.x;
                rotation.z = transform.rotation.z;
                break;
            case Axis.z:
                rotation.x = transform.rotation.x;
                rotation.y = transform.rotation.y;
                break;
        }

        transform.rotation = rotation;
    }
}