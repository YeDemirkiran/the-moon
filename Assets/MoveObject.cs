using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] float speed = 2f;

    Vector3 position;

    private void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        position += transform.forward * speed * Time.unscaledDeltaTime;
        transform.position = position;
    }
}