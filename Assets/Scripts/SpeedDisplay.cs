using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedDisplay : MonoBehaviour
{
    Text speedDisplay;
    // Start is called before the first frame update
    void Start()
    {
        speedDisplay = this.GetComponentInChildren<Text>();
        speedDisplay.text = "0.0 u/s";
    }

    public void WriteSpeed(float newSpeed)
    {
        string speedText = "";

        speedText += newSpeed.ToString();

        speedText += " u/s";

        speedDisplay.text = speedText;
    }
}
