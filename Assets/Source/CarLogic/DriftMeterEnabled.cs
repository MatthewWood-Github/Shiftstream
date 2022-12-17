using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriftMeterEnabled : MonoBehaviour
{
    public Slider driftMeter;
    void Update()
    {
        driftMeter.gameObject.SetActive(PlayerSettings.Settings.driftMeterEnabled);
    }
}
