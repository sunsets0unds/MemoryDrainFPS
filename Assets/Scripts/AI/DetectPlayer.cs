using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [HideInInspector]
    public bool playerFound = false;
    [SerializeField]
    private PlayerManager player;
    [Range(15, 80)]
    public float maxCone = 20f;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    private void OnDrawGizmosSelected()
    {
        Matrix4x4 temp = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, maxCone, this.GetComponent<SphereCollider>().radius, 0, 1);
        Gizmos.matrix = temp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 targetDir = player.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);

            RaycastHit hit;

            if (angle < maxCone)
            {
                if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, this.GetComponent<SphereCollider>().radius) && hit.transform.tag == "Player")
                {
                    Debug.DrawRay(transform.position, hit.point, Color.green);
                    playerFound = true;
                }
                else
                {
                    Debug.DrawRay(transform.position, hit.point, Color.red);
                    playerFound = false;
                }

            }
            else
            {
                playerFound = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 targetDir = player.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);

            RaycastHit hit;

            if (angle < maxCone)
            {
                if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, this.GetComponent<SphereCollider>().radius) && hit.transform.tag == "Player")
                {
                    Debug.DrawRay(transform.position, hit.point, Color.green);
                    playerFound = true;
                }
                else
                {
                    Debug.DrawRay(transform.position, hit.point, Color.red);
                    playerFound = false;
                }
            }
            else
            {
                playerFound = false;
            }
        }
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
