using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary> Manages the values of the player's HUD elements. </summary>
public class UIManager : MonoBehaviour
{
    [Header("GameObjects")]
    /// <summary> Current car. </summary>
    [SerializeField] private GameObject car;

    /// <summary> Camera object. </summary>
    [SerializeField] private PlayerCamera mainCamera;

    /// <summary> Current elapsed time. </summary>
    public float timeInSeconds { get; private set; }

    /// <summary> Formatted string to apply to UI. </summary>
    private string text;

    /// <summary> Time converted from <see cref="timeInSeconds"/>. </summary>
    private TimeSpan time;

    [Header("UI")]
    /// <summary> Drift indicator UI element. </summary>
    [SerializeField] private Slider driftMeter;
    /// <summary> Timer UI element. </summary>
    [SerializeField] private TMP_Text timerText;

    /// <summary> Lap counter UI element. </summary>
    [SerializeField] private TMP_Text lapCounterText;

    /// <summary> Timer UI element. </summary>
    [SerializeField] private TMP_Text speedometerText;

    /// <summary> Timer UI element's shadow. </summary>
    [SerializeField] private TMP_Text timerTextShadow;

    /// <summary> Lap counter UI element's shadow. </summary>
    [SerializeField] private TMP_Text lapCounterTextShadow;

    /// <summary> Timer UI element's shadow. </summary>
    [SerializeField] private TMP_Text speedometerTextShadow;

    [Header("Car Components")]
    /// <summary> Lap counter component. </summary>
    private LapCounter lapCounter;

    /// <summary> Movement component. </summary>
    private BaseMove move;

    /// <summary> Drift component. </summary>
    private Drift drift;

    private Timer timer;


    /// <summary> Sets the timerText UI element equal to total time passed. </summary>
    /// <remarks> Rounds time to the nearest second. </remarks>
    private void Timer()
    {
        time = timer.time;

        text = string.Format("{0:00}:{1:00}:{2:00}", Math.Truncate(time.TotalHours), Math.Truncate(time.TotalMinutes), time.Seconds);
        timerText.text = text;
        timerTextShadow.text = text;
    }

    /// <summary> Sets the driftMeter UI element equal to the current drift progress. </summary>
    /// <remarks> Rounds down the the nearest integer. </remarks>
    private void DriftSlider()
    {
        drift = car.GetComponent<Drift>();

        driftMeter.maxValue = drift.maxDriftPercent;
        driftMeter.value = Mathf.FloorToInt(drift.currentDriftPercent);
    }

    /// <summary> Sets the speedometerText UI element to the car's current speed. </summary>
    private void Speedometer()
    {   
        move = car.GetComponent<CarSettings>().currentMovement;

        String newText = string.Format("{0} MPH", Math.Abs(move.GetMovementSpeed()).ToString("F0"));
        speedometerText.text = newText;
        speedometerTextShadow.text = newText;
    }

    /// <summary> Sets the lapCounterText UI element to the car's current lap. </summary>
    private void LapCount()
    {
        lapCounter = car.GetComponent<LapCounter>();

        String newText = string.Format("Lap: {0}", lapCounter.GetLapCount());
        lapCounterText.text = newText;
        lapCounterTextShadow.text = newText;
    }

    private void Start()
    {
        this.lapCounter = this.GetComponent<LapCounter>();
        this.timer = GameObject.Find("RaceManager").GetComponent<Timer>();
    }

    void Update()
    {
        // Shows the details of the car which the camera is attached to.
        car = mainCamera.targetCar;

        Timer();
        DriftSlider();
        Speedometer();
        LapCount();
    }
}
