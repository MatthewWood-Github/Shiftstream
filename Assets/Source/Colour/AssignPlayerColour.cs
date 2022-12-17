using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssignPlayerColour : MonoBehaviour
{
    private void Update()
    {
        this.GetComponent<Image>().color = PlayerSettings.Settings.HudColour;

        if (this.transform.Find("Text").GetComponent<TextMeshProUGUI>() == true)
            this.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = PlayerSettings.Settings.HudColour;
    }
}
