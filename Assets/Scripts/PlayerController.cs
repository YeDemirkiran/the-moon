using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] new Transform camera;
    [SerializeField] ParentFollower flashlight;
    [SerializeField] LayerMask bugLayerMask;
    public AudioSource audioSource;
    public AudioClip scaredClip;
    public FirstPersonController fpsController {  get; private set; }

    Coroutine body, cam;

    public bool rotationStarted { get; set; } = false;
    public bool rotationComplete { get; set; } = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fpsController = GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        RevertBug();
    }

    void RevertBug()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, Mathf.Infinity, bugLayerMask))
        {
            Bug bug = hit.collider.transform.GetComponentInParent<Bug>();
            if (bug != null)
            {
                bug.animator.SetFloat("speed", -2f);
            }
        }
    }

    public void RotateTowards(Vector3 point, float duration)
    {
        rotationStarted = true;
        rotationComplete = false;

        if (body != null)
        {
            StopCoroutine(body); 
        }

        body = StartCoroutine(RotateBody(point, duration));

        if (cam != null)
        {
            StopCoroutine(cam);        
        }

        cam = StartCoroutine(RotateCamera(point, duration));
    }

    IEnumerator RotateBody(Vector3 point, float duration)
    {
        fpsController.enabled = false;

        float lerp = 0f;

        Vector3 rotation = transform.eulerAngles;
        Vector3 startRotation = rotation;

        Vector3 direction = (point - transform.position).normalized;
        Vector3 targetRotaton = Quaternion.LookRotation(direction).eulerAngles;
        targetRotaton.x = rotation.x;
        targetRotaton.z = rotation.z;

        while (lerp <= 1f)
        {
            lerp += Time.deltaTime / duration;

            rotation = Vector3.Slerp(startRotation, targetRotaton, lerp);
            transform.eulerAngles = rotation;

            yield return null;
        }

        fpsController.enabled = true;
        rotationComplete = true;
    }

    IEnumerator RotateCamera(Vector3 point, float duration)
    {
        fpsController.enabled = false;

        float lerp = 0f;

        Vector3 euler = camera.localEulerAngles;
        Quaternion startRotation = Quaternion.Euler(euler);

        Vector3 direction = (point - camera.position).normalized;
        Vector3 targetEuler = Quaternion.LookRotation(direction).eulerAngles;
        targetEuler.y = euler.y;
        targetEuler.z = euler.z;
        Quaternion targetRotation = Quaternion.Euler(targetEuler);

        float originalSpeed = flashlight.rotationSpeed;
        flashlight.rotationSpeed *= 10f;

        while (lerp <= 1f)
        {
            lerp += Time.deltaTime / duration;

            euler = Quaternion.Slerp(startRotation, targetRotation, lerp).eulerAngles;
            camera.localEulerAngles = euler;

            yield return null;
        }

        fpsController.pitch = targetEuler.x;
        fpsController.enabled = true;

        rotationComplete = true;

        flashlight.rotationSpeed = originalSpeed;
    }
}