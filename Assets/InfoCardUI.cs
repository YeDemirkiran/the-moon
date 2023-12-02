using System;
using TMPro;
using UnityEngine;

public class InfoCardUI : MonoBehaviour
{
    [SerializeField] TMP_Text hourText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hourText.text = $"{DateTime.Now.ToString("HH:mm:ss tt")}";
    }
}