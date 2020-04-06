using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePortal : MonoBehaviour
{
    [Tooltip("The index for the scene this goes to"), Min(0)]
    public int sceneIndex = 0;
    public Text interactable;
    public KeyCode interactKey;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            interactable.enabled = true;
            interactable.text = "Press " + interactKey.ToString() + " to end level";

            if(Input.GetKeyDown(interactKey))
                GameManager.ChangeLevel(sceneIndex);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(interactKey))
            GameManager.ChangeLevel(sceneIndex);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            interactable.enabled = false;
        }
    }
}
