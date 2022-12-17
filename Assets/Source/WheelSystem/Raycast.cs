using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] public Rigidbody car;

    public float travel;
    public float stiffness;
    public float damper;
    public float wheelRadius;
    public float springVelocity;
    public bool grounded;
    float lastLength;
    RaycastHit hit;

    public string groundTag;

    private float SpringForce()
    {
        return stiffness * (travel - (hit.distance - wheelRadius));
    }

    private float DamperForce()
    {
        return damper * springVelocity;
    }

    private void FireRaycast()
    {
        lastLength = travel - hit.distance - wheelRadius;
        if (Physics.Raycast(this.transform.position, -transform.up, out hit, travel + wheelRadius))
        {
            springVelocity = ((travel - hit.distance - wheelRadius) - lastLength) / Time.fixedDeltaTime;
            car.AddForceAtPosition(transform.up * (SpringForce() + DamperForce()), this.transform.position, ForceMode.Force);

            grounded = true;
            groundTag = hit.transform.tag;

            Debug.DrawRay(this.transform.position, -transform.up * hit.distance, Color.green);
        }
        else
        {
            grounded = false;

            Debug.DrawRay(this.transform.position, -transform.up * (travel + wheelRadius), Color.red);
        }
    }

    public bool GetGrounded()
    {
        return grounded;
    }

    public string GetTag()
    {
        return groundTag;
    }

    private void FixedUpdate()
    {
        FireRaycast();
    }
}
