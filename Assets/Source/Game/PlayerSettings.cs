using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings Settings { get; set; }
    public float r = 1.0f;
    public float g = 1.0f;
    public float b = 1.0f;
    public Color HudColour;

    public bool driftMeterEnabled = true;

    public float currentTime;
    public float fastestTime;

    private void Awake()
    {
        if (Settings != null && Settings != this)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Settings = this;
            HudColour = new Color(r, g, b);
        }
    }
}