using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    Text ammoDisplay;

    // Start is called before the first frame update
    void Start()
    {
        ammoDisplay = GetComponent<Text>();
        ammoDisplay.text = "HP: 0";
    }

    public void WriteAmmo(int newAmmo)
    {
        string newAmmoText = "Ammo: ";

        newAmmoText += newAmmo.ToString();

        ammoDisplay.text = newAmmoText;
    }
}
