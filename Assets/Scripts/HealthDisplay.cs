using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Text healthDisplay;

    // Start is called before the first frame update
    void Start()
    {
        healthDisplay = GetComponent<Text>();
        healthDisplay.text = "HP: 0";
    }

    public void WriteHealth(int newHealth)
    {
        string newHealthText = "HP: ";

        newHealthText += newHealth.ToString();

        healthDisplay.text = newHealthText;
    }
}
