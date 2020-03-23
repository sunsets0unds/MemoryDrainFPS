using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region public variables
    public float health = 100;
    public int maxHealth = 100;
    public int ammo = 100;
    public int maxAmmo = 150;
    [HideInInspector]
    public bool haveAmmo;
    private HealthDisplay healthDisplayObj;
    private AmmoDisplay ammoDisplayObj;
    #endregion

    private void Start()
    {
        try
        {
            healthDisplayObj = FindObjectOfType<HealthDisplay>();
        }
        catch(NullReferenceException e)
        { }

        try
        {
            ammoDisplayObj = FindObjectOfType<AmmoDisplay>();
        }
        catch(NullReferenceException e)
        { }
    }

    // Update is called once per frame
    void Update()
    {
        if (health > maxHealth)
            health = maxHealth;
        if (ammo > maxAmmo)
            ammo = maxAmmo;
        if (ammo > 0)
            haveAmmo = true;
        else
            haveAmmo = false;

        if(healthDisplayObj)
        {
            healthDisplayObj.WriteHealth((int)health);
        }

        if(ammoDisplayObj)
        {
            ammoDisplayObj.WriteAmmo(ammo);
        }
    }

    public void playerDamage(float damage)
    {
        this.health -= damage;
    }
}
