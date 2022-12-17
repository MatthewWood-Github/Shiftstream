using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    /// <summary> The rigidbody of the current object. </summary>
    private Rigidbody car;

    /// <summary> Current speed in units/second. </summary>
    public float movementSpeed { get; private set; }

    /// <summary> The maximum speed that the car can travel forwards. </summary>
    public float maxMovementSpeed { get; } = 60;

    /// <summary> The maximum speed that the car can travel whilst reversing. </summary>
    [SerializeField] protected float maxReverseSpeed = -20;

    /// <summary> The current speed of the car. </summary>
    [SerializeField] protected float currentAcceleration;

    /// <summary> The rate at which the car accelerates. 
    /// DESIGNED TO MODIFY
    /// </summary>
    public float accelerationRate = 1.33f;

    /// <summary> 
    /// Maximum rate of acceleration. Equal to max MPH / 100 
    /// DESIGNED TO MODIFY
    /// </summary>
    public float maxAcceleration = 1.33f;

    /// <summary> 
    /// Maximum speed the car can travel offroad. 
    /// DESIGNED TO MODIFY
    /// </summary>
    protected float maxOffroadSpeed = 20f;

    /// <summary> Acceleration rate of the car whilst reversing. </summary>
    [SerializeField] protected float reverseRate = 0.25f;

    /// <summary> 
    /// Amount of torque applied to the car whilst steering. 
    /// DESIGNED TO MODIFY
    /// </summary>
    [SerializeField] protected float torqueForce = 5000f;

    /// <summary> Sets the maximum angular velocity of the rigidbody. </summary>
    protected float maxAngularVelocity = 2;

    /// <summary> The percentage of "<see cref="maxMovementSpeed"/>" and "<see cref="maxReverseSpeed"/>" that the car can begin to steer. </summary>
    [SerializeField] protected float steerToSpeedRatio = 5;

    /// <summary> Vertical input. </summary>
    public float vertical { get; protected set; }

    /// <summary> Horizontal input. </summary>
    public float horizontal { get; protected set; }

    /// <summary> Is the car reversing? </summary>
    protected bool reversing;

    /// <summary> Is the car braking? </summary>
    public bool braking;

    public float brakingRate;

    public float decelerationRate;

    /// <summary> Is the car grounded? </summary>
    public bool grounded { get; private set; }

    /// <summary> Is the car drifting?. </summary>
    public bool drifting { get; set; }

    public bool jumping { get; protected set; }

    /// <summary> Is the car offroad? </summary>
    public bool isOffroad { get; private set; }

    public bool boosting { get; set; }

    /// <summary> Force to be applied to cancel sideways movement. </summary>
    public float sideSpeed { get; private set; }

    /// <summary> Limits the extent speed that the car based on set criteria. </summary>
    protected void Constraints()
    {
        if (isOffroad)
            car.velocity = Vector3.ClampMagnitude(car.velocity, maxOffroadSpeed);
            
        if (!reversing)
        {
            currentAcceleration = Mathf.Clamp(currentAcceleration, 0, maxAcceleration);
            if (currentAcceleration < 0f) currentAcceleration = 0;
        }
            
            
        else if (reversing)
        { 
            if (movementSpeed < 0f)
                car.velocity = Vector3.ClampMagnitude(car.velocity, maxReverseSpeed);
            
            currentAcceleration = Mathf.Clamp(currentAcceleration, 0, -maxReverseSpeed);
        }
    }

    /// <summary> Handler for movement states based on vertical input. </summary>
    /// <remarks> Each movement state uses their own function. If in the air, decelerate </remarks>
    protected void Movement()
    {
        switch (vertical)
        {
            case -1:
                Reverse();
                break;
            case 0:
                Decelerate();
                break;
            case 1:
                Accelerate();
                break;
        }
    }

    /// <summary> Accelerates the car forwards by increasing the movement speed. </summary>
    /// <remarks>
    /// Only occurs if car is grounded. Otherwise the car decelerates using <see cref="Decelerate"/>.
    /// Sets reversing to false.
    /// </remarks>>
    protected void Accelerate()
    {      
        reversing = false;
        currentAcceleration += accelerationRate * Time.deltaTime;
    }

    /// <summary> Slows down the car to a stop. </summary>
    /// <remarks> Sets reversing to false. </remarks>
    protected void Decelerate()
    {
        reversing = false;
        
        if (braking) currentAcceleration -= brakingRate * Time.deltaTime;
        else if (!braking) currentAcceleration -= decelerationRate * Time.deltaTime; 
    }

    /// <summary> Accelerates the car backwards. </summary>
    /// <remarks> Sets reversing to true. </remarks>

    protected void Reverse()
    {
        reversing = true;
        currentAcceleration -= reverseRate * Time.deltaTime;
    }

    /// <summary> Constantly applies a driving force based on "<see cref="currentAcceleration"/>". </summary>

    protected void DrivingForce()
    {
        car.AddRelativeForce(Vector3.forward * currentAcceleration * Time.deltaTime, ForceMode.VelocityChange);
    }

    /// <summary> Applies torque depending on horizontal input. </summary>

    protected void Steering()
    {
        if (horizontal == 0)
            car.AddRelativeTorque(car.angularVelocity * -torqueForce * Time.deltaTime);
        if (vertical == -1 && movementSpeed <= 0)
            car.AddRelativeTorque(this.transform.up * horizontal * -torqueForce * Time.deltaTime);
        else if (movementSpeed >= maxMovementSpeed * steerToSpeedRatio)
            car.AddRelativeTorque(this.transform.up * horizontal * torqueForce * Time.deltaTime);
    }

    /// <summary> Calculates the movement speed from the relative velocity. </summary>
    /// <remarks> Movement speed is read only. </remarks>

    protected void CalculateMovementSpeed()
    {
        Vector3 relativeVelocity = Quaternion.Inverse(transform.rotation) * car.velocity;
        float speed = relativeVelocity.z;

        sideSpeed = relativeVelocity.x;
        sideSpeed = (float)Mathf.Round(sideSpeed * 100f) / 100f;

        movementSpeed = (float)Mathf.Round(speed * 100f) / 100f;
    }

    /// <summary> Sets the grounded variable. </summary>

    protected void SetGrounded()
    {
        this.grounded = this.GetComponent<GroundCheck>().IsGrounded();
    }

    protected void GetOffroad()
    {
        isOffroad = this.GetComponent<GroundCheck>().IsOffroad();
    }

    public float GetMovementSpeed()
    {
        return movementSpeed * 2;
    }

    protected void Start()
    {
        car = GetComponent<Rigidbody>();
        car.centerOfMass = new Vector3(0, -1f, 0);
    }

    protected void Update()
    {
        car.maxAngularVelocity = maxAngularVelocity;
        Constraints();
        Movement();
        CalculateMovementSpeed();
        SetGrounded();
        GetOffroad();
    }

    protected void FixedUpdate()
    {
        DrivingForce();
        Steering();
    }
}
