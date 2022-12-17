using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider slider_r;
    public Slider slider_g;
    public Slider slider_b;

    public float r;
    public float g;
    public float b;

    public Image menu;

    public Color colour;

    public Button restartButton;
    public Button settingsButton;
    public Button menuButton;

    public Toggle driftToggle;
    public Slider driftMeter;

    public PlayerSettings settings;

    private void AssignColourValues()
    {
        settings.r = slider_r.value;
        settings.g = slider_g.value;
        settings.b = slider_b.value;
    }

    private void SetColour()
    {
        colour = new Color(settings.r, settings.g, settings.b);
        settings.HudColour = colour;
    }

    private void ApplyColour()
    {
        menu.color = colour;

        slider_r.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = settings.HudColour;
        slider_g.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = settings.HudColour;
        slider_b.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = settings.HudColour;
    }

    public void Toggles()
    {
        settings.driftMeterEnabled = driftToggle.isOn;
    }

    private void Start()
    {
        settings = PlayerSettings.Settings;
        slider_r.value = settings.r;
        slider_g.value = settings.g;
        slider_b.value = settings.b;

        driftToggle.isOn = settings.driftMeterEnabled;
        menu = this.GetComponent<Image>();
    }

    void Update()
    {
        AssignColourValues();
        SetColour();
        ApplyColour();
        Toggles();
    }
}
