using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private bool wallCollide;

    public Vector3 CheckStep(Vector3 currentVelocity, Rigidbody body)
    {
        //calculate if player will collide with something
        float distance = currentVelocity.magnitude * Time.fixedDeltaTime;

        RaycastHit hit;

        if (body.SweepTest(currentVelocity, out hit, distance) && wallCollide)
            return new Vector3(0, body.velocity.y, 0);

        return currentVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            wallCollide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            wallCollide = false;
        }
    }
}
