using UnityEngine;

public class EarthRotation : MonoBehaviour
{
    [SerializeField] Axis axis;
    [SerializeField] float degreesPerSecond, pivotMultiplier;
    [SerializeField] Transform centerPivot;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused) return;

        Vector3 delta = new Vector3(0f, 0f, 0f);

        switch (axis)
        {
            case Axis.x:
                delta.x = degreesPerSecond;
                break;
            case Axis.y:
                delta.y = degreesPerSecond;
                break;
            case Axis.z:
                delta.z = degreesPerSecond;
                break;
        }

        transform.eulerAngles += delta * Time.deltaTime;

        transform.RotateAround(centerPivot.position, Vector3.up, degreesPerSecond * pivotMultiplier * Time.deltaTime);
    }
}