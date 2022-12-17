using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public float timeInSeconds { get; private set; }
    public TimeSpan time { get; private set; }
    private void RaceTimer()
    {
        timeInSeconds += Time.deltaTime;
        time = TimeSpan.FromSeconds(timeInSeconds);
    }

    void Update()
    {
        RaceTimer();
    }
}
