using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Handles the animations of the car. </summary>
public class CarAnimations : MonoBehaviour
{
    [Header("Components")]

    /// <summary> The car to apply animations to. </summary>
    [SerializeField] private BaseMove car;

    /// <summary> The drift component of the current car. </summary>
    [SerializeField] private Drift drift;

    [Header("Turn Values")]

    /// <summary> Rate that the front wheels turn. </summary>
    [SerializeField] protected float wheelTurnRate;

    /// <summary> Current angle of the front wheels. </summary>
    [SerializeField] private float wheelAngle;

    /// <summary> Maximum angle that the front wheels can turn. </summary>
    [SerializeField] private float maxWheelAngle;

    [SerializeField] private float wheelSpinRate;

    [Header("Particle Systems")]

    /// <summary> Particle system for boosting. </summary>
    [SerializeField] private ParticleSystem fireTrail;

    /// <summary> Particle system for explosions. </summary>
    [SerializeField] private ParticleSystem explosion;

    /// <summary> Particle system for wheels while drifting. </summary>
    [SerializeField] private ParticleSystem[] sparks;

    /// <summary> Particle system for wheels while drifting. </summary>
    [SerializeField] private ParticleSystem[] flashes;

    /// <summary> Particle system for drift stage transitions. </summary>
    [SerializeField] private ParticleSystem[] bigFlashes;

    /// <summary> Particle system for tire marks while boosting and driftiing. </summary>
    [SerializeField] private TrailRenderer[] trails;

    [Header("Wheels")]

    /// <summary> Parent object for all wheels. </summary>
    [SerializeField] private Transform wheelParent;

    /// <summary> List of the car's wheels. </summary>
    [SerializeField] private List<GameObject> wheels;

    [Header("Fire Once")]

    /// <summary> Check if the stage 1 drift animation has played. </summary>
    private bool flashed = false;

    /// <summary> Check if the stage 2 drift animation has played. </summary>
    private bool flashed2 = false;

    /// <summary> Check if the initial boost animation has played. </summary>
    private bool exploded = false;

    /// <summary> Manages spark animations played during a drift. </summary>
    /// <remarks> 
    ///     Changes colour based on drift progess. <br/>
    ///     Plays an animation between drift stages.
    /// </remarks>
    protected void SparkManager()
    {
        foreach (ParticleSystem spark in sparks)
        {
            // If in the first drift stage.
            if (drift.currentDriftPercent > 1 && drift.currentDriftPercent < 2)
            {
                // Lighter blue
                Color startColour1 = new Color(0.13f, 0.92f, 1f);

                // Darker blue
                Color startColour2 = new Color(0.58f, 0.58f, 1f);

                // Play flash animation once per drift stage.
                if (flashed == false)
                {
                    foreach (ParticleSystem flash in bigFlashes)
                        flash.Play();

                    flashed = true;
                }

                foreach (ParticleSystem flash in flashes)
                {
                    var main = flash.main;
                    main.startColor = new ParticleSystem.MinMaxGradient(startColour1, startColour2);

                    flash.Play();
                }

                // Reset spark colours
                var sparkMain = spark.main;
                sparkMain.startColor = new ParticleSystem.MinMaxGradient(startColour1, startColour2);

                spark.Play();
            }

            // If in the second drift stage
            else if (drift.currentDriftPercent >= 2)
            {
                // Lighter Orange
                Color startColour1 = new Color(1f, 0.8f, 0.0f);

                // Darker Orange
                Color startColour2 = new Color(0.9f, 0.65f, 0f);

                // Play flash animation once per drift stage.
                if (flashed2 == false)
                {
                    foreach (ParticleSystem flash in bigFlashes)
                        flash.Play();

                    flashed2 = true;
                }

                foreach (ParticleSystem flash in flashes)
                {
                    var main = flash.main;
                    main.startColor = new ParticleSystem.MinMaxGradient(startColour1, startColour2);

                    flash.Play();
                }

                // Set new spark colours.
                var sparkMain = spark.main;
                sparkMain.startColor = new ParticleSystem.MinMaxGradient(startColour1, startColour2);

                spark.Play();

            }
            // If not drifting
            else
                spark.Stop();
        }
    }

    /// <summary> Manages boosting and tire mark animations. </summary>
    /// <remarks> 
    ///     Deploys tire marks while boosting or drifting. <br/>
    ///     Plays explosion and fire trail animations while boosting.
    /// </remarks>
    protected void TrailManager()
    {
        if ((car.drifting || car.boosting) && car.grounded)
        {
            foreach (TrailRenderer trail in trails)
                trail.emitting = true;
        }
        else
        {
            foreach (TrailRenderer trail in trails)
                trail.emitting = false;
        }

        if (!car.drifting && car.boosting)
        {
            // Play explosion once.
            if (exploded == false)
            {
                explosion.Play();
                exploded = true;
            }
        }
        // If not boosting or are drifting, let the explosion play again.
        else
            exploded = false;
        
        // Fire trail while boosting.
        if (car.boosting) fireTrail.Play();
        else fireTrail.Stop();

        if (car.drifting)
            exploded = false;
    }

    /// <summary> Animation controller for the front wheels. </summary>
    /// <remarks> Turns wheel depending on steering direction. </remarks>
    protected void WheelTurn()
    {
        wheelAngle = Mathf.Clamp(wheelAngle, -maxWheelAngle + 1, maxWheelAngle - 1);

        if (car.horizontal > 0)
            wheelAngle += wheelTurnRate * Time.deltaTime;

        else if (car.horizontal < 0)
            wheelAngle -= wheelTurnRate * Time.deltaTime;

        // Apply rotation to front wheels.
        foreach (GameObject wheel in wheels)
        {
            if (wheel.tag != "FrontWheel") break;

            if (wheelAngle <= maxWheelAngle - 1 && car.horizontal > 0)
                wheel.transform.Rotate(new Vector3(0f, wheelTurnRate, 0f) * Time.deltaTime);

            else if (wheelAngle >= -maxWheelAngle + 1 && car.horizontal < 0)
                wheel.transform.Rotate(new Vector3(0f, -wheelTurnRate, 0f) * Time.deltaTime);
            
        }

        // Revert the front wheel rotation if a rotation exists.
        if (car.horizontal == 0 && wheelAngle != 0)
        {
            foreach (GameObject wheel in wheels)
            {
                if (wheel.tag != "FrontWheel") break;

                if (wheelAngle >= 1) wheel.transform.Rotate(new Vector3(0f, -wheelTurnRate, 0f) * Time.deltaTime);

                else if (wheelAngle <= -1) wheel.transform.Rotate(new Vector3(0f, wheelTurnRate, 0f) * Time.deltaTime);
            }

            // Reset wheel angle variable over time.
            wheelAngle = Mathf.MoveTowards(wheelAngle, 0f, wheelTurnRate * Time.deltaTime);
        }
    }

    /// <summary> Spin wheels to mimic rotation. </summary>
    private void WheelSpin()
    {
        foreach (GameObject wheel in wheels)
        {
            wheel.transform.Rotate(0, 0, (-wheelSpinRate * car.movementSpeed) + (car.vertical * -wheelSpinRate) * Time.deltaTime, Space.Self);

            if (wheel.transform.tag == "FrontWheel")
                wheel.transform.localRotation = Quaternion.Euler(0, wheelAngle + 180, wheel.transform.localEulerAngles.z);
            else if (wheel.transform.tag != "FrontWheel")
                wheel.transform.localRotation = Quaternion.Euler(0, 180, wheel.transform.localEulerAngles.z);
        }
    }

    /// <summary> Allows flash animations to be used again if not drifting. </summary>
    private void AllowFlashAnimations()
    {
        if (car.drifting) return;
        
        flashed = false;
        flashed2 = false;
    }
    void Start()
    {
        car = this.GetComponent<BaseMove>();

        drift = this.GetComponent<Drift>();

        wheelParent = this.transform.Find("Wheels");

        // Add wheel objects to wheels list.
        for (int x = 0; x < wheelParent.transform.childCount; x++)
            wheels.Add(wheelParent.GetChild(x).gameObject);
    }

    void Update()
    {
        car = this.GetComponent<CarSettings>().currentMovement;

        WheelTurn();
        WheelSpin();
        TrailManager();
        SparkManager();
        AllowFlashAnimations();
    }
}
