using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGround;
    [SerializeField]
    private string groundTag = "Ground";
    [SerializeField]
    private LayerMask groundLayers;
    [SerializeField, Range(1, 90)]
    private float maxAngle = 60;


    // Start is called before the first frame update
    void Start()
    {
        isGround = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == groundTag)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1, groundLayers))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle < maxAngle)
                    isGround = true;
                else
                    isGround = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == groundTag)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 1, groundLayers))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle < 60)
                    isGround = true;
                else
                    isGround = false;
            }
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
