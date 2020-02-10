using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGround;
    [SerializeField]
    private string groundTag = "Ground";

    // Start is called before the first frame update
    void Start()
    {
        isGround = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == groundTag)
        {
            isGround = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == groundTag)
        {
            isGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == groundTag)
        {
            isGround = false;
        }
    }
}
