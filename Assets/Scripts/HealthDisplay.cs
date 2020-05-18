using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Text healthDisplay;
    Slider bar;

    // Start is called before the first frame update
    void Start()
    {
        healthDisplay = GetComponent<Text>();
        bar = GetComponentInChildren<Slider>();
        healthDisplay.text = "HP: 0";
        bar.value = 0;
    }

    public void WriteHealth(int newHealth)
    {
        string newHealthText = "HP: ";
        float newHealthValue = 0;

        newHealthText += newHealth.ToString();

        newHealthValue = (float)newHealth / 100f;

        healthDisplay.text = newHealthText;
        bar.value = newHealthValue;
    }
}
