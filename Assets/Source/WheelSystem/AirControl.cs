using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Keeps GameObject horizontal whilst airborne. </summary>

public class AirControl : MonoBehaviour
{
    /// <summary> Movement script of this GameObject. </summary>
    private BaseMove car;

    /// <summary> Rigidbody of this GameObject. </summary>
    private Rigidbody rb;

    /// <summary> The amount the car should be able to move whilst airborne. </summary>
    [SerializeField] protected float stability = 1f;

    /// <summary> Speed that the rotation applies. </summary>
    [SerializeField] protected float speed = 20.0f;

    /// <summary> Applies counter-rotation whilst airborne. </summary>
    protected void StabiliseAirRotation()
    {
        if (car.grounded == true) return;
        
        Vector3 predictedUp = Quaternion.AngleAxis(
            rb.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            rb.angularVelocity
            ) * transform.up;

        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        rb.AddTorque(torqueVector * speed * speed);
        
    }

    private void Start()
    {
        car = this.GetComponent<BaseMove>();
        rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        StabiliseAirRotation();
    }
}
