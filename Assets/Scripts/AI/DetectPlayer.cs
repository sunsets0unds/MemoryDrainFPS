using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [HideInInspector]
    public bool playerFound = false;
    private PlayerManager player;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            playerFound = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playerFound = false;
    }

    public Vector3 findPlayerInScene()
    {
        return player.gameObject.transform.position;
    }
}
