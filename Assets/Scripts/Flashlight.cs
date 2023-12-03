using UnityEngine;

public class Flashlight : MonoBehaviour
{
    new Light light;

    [SerializeField] bool open = true;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        light.enabled = open;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            open = !open;

            light.enabled = open;
        }
    }
}