using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int amount = 50;
    public AudioClip pickupSound;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();

            if (playerManager.ammo < 200)
            {
                playerManager.ammo += amount;
                playerManager.source.clip = pickupSound;
                playerManager.source.Play();
                Destroy(this.gameObject);
            }
        }
    }
}
