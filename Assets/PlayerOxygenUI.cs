using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygenUI : MonoBehaviour
{
    PlayerOxygen oxygen;

    [SerializeField] Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        oxygen = PlayerOxygen.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = oxygen.currentOxygen / oxygen.maxOxygen;
    }
}