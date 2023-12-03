using System;
using TMPro;
using UnityEngine;

public class InfoCardUI : MonoBehaviour
{
    [SerializeField] TMP_Text hourText;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isPaused) return;
        hourText.text = $"{DateTime.Now.ToString("hh:mm:ss tt")}";
    }
}