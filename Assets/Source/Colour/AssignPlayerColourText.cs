using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssignPlayerColourText : MonoBehaviour
{
    void Update()
    {
        this.GetComponent<TextMeshProUGUI>().color = PlayerSettings.Settings.HudColour;
    }
}
