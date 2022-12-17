using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BaseMove car;

    [Header("Drifting")]

    /// <summary> Minimum speed during a drift before countermeasures are applied. </summary>
    protected float driftSpeedThreshhold = 10;

    /// <summary> 
    /// Force to apply after a drift 
    /// DESIGNED TO MODIFY
    /// </summary>
    [SerializeField] protected float driftForce = 0.5f;

    /// <summary> Current percentage of drift </summary>
    public float currentDriftPercent { get; set; }

    /// <summary> Maximum force to apply after a drift </summary>
    public float maxDriftPercent { get; } = 2;

    /// <summary> 
    /// Rate that the drift force increases 
    /// DESIGNED TO MODIFY
    /// </summary>
    [SerializeField] protected float driftForceRate = 0.025f;

    protected float driftTimer = 0f;

    protected float maxDriftTimer = 1.5f;

    protected float driftMultiplier = 1;

    [SerializeField] private float tireGrip = 0.05f; // DESIGNED TO MODIFY

    [SerializeField] private float driftGripRatio = 0.2f; // DESIGNED TO MODIFY

    [SerializeField] private float currentGrip;

    /// <summary> Ensures the car travels in a straight line unless drifting. </summary>

    protected void DriftManager()
    {
        if (car.movementSpeed < driftSpeedThreshhold)
            car.boosting = false;

        if (car.isOffroad)
        {
            car.drifting = false;
            currentDriftPercent = 0;
        }
        
        applyCounterForce();
        SetCurrentGrip();
        IncreaseDriftPercent();
    }

    private void applyCounterForce()
    {
        if (car.sideSpeed > 5 && car.sideSpeed < -5 && car.sideSpeed != 0)
            rb.AddRelativeForce(Vector3.right * Mathf.Sign(car.sideSpeed) * Time.deltaTime, ForceMode.VelocityChange);
        else
            rb.AddRelativeForce(Vector3.right * -(car.sideSpeed * currentGrip) * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void SetCurrentGrip()
    {
        if (!car.drifting)
            currentGrip = tireGrip;

        else if (car.drifting)
            currentGrip = tireGrip * driftGripRatio * Time.deltaTime;
    }

    private void IncreaseDriftPercent()
    {
        if (car.drifting)
        {
            currentDriftPercent = Mathf.Clamp(currentDriftPercent, 0, maxDriftPercent);

            currentDriftPercent += driftForceRate * Mathf.Abs(car.horizontal) * Time.deltaTime;
        }
    }

    protected void DriftBoostTimer()
    {
        // Activates boosting.
        if (currentDriftPercent > 0 && !car.drifting && !car.isOffroad)
        {
            driftMultiplier = Mathf.FloorToInt(currentDriftPercent);
            currentDriftPercent = 0;
            driftTimer = 0;
            car.boosting = true;
        }

        // Increments the timer.
        if (car.boosting)
        {
            driftTimer += Time.deltaTime;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, car.maxMovementSpeed * 1.5f);
        }

        // Stops boosting.
        if (driftTimer > maxDriftTimer * driftMultiplier)
        {
            driftTimer = 0;
            car.boosting = false;
        }
    }

    protected void DriftBoost()
    {
        if (car.boosting)
            rb.AddRelativeForce(driftForce * Vector3.forward * Time.deltaTime, ForceMode.VelocityChange);
    } 

    void Start()
    {
        car = this.GetComponent<BaseMove>();
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        car = this.GetComponent<CarSettings>().currentMovement;
        DriftBoostTimer();
    }

    private void FixedUpdate()
    {
        DriftManager();
        DriftBoost();
    }
}
