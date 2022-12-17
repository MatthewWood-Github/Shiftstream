using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DisplayFastestTime : MonoBehaviour
{
    public float currentTimeBetweenFrames = 0f;
    public float timeBetweenFrames = 3f;
    public int frameIndex = 0;

    public GameObject winScreen;
    public GameObject timer;

    public UIManager ui;
    public PauseMenu pause;

    public List<GameObject> screens;

    private void Comparison()
    {
        if (PlayerSettings.Settings.currentTime <= PlayerSettings.Settings.fastestTime)
        {
            winScreen.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "WIN!";
            winScreen.transform.Find("Message-Shadow").GetComponent<TextMeshProUGUI>().text = "WIN!";

            timer.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "New Fastest Time!";
            timer.transform.Find("Message-Shadow").GetComponent<TextMeshProUGUI>().text = "New Fastest Time!";
        }

        else if (PlayerSettings.Settings.currentTime > PlayerSettings.Settings.fastestTime)
        {
            winScreen.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "FAIL!";
            winScreen.transform.Find("Message-Shadow").GetComponent<TextMeshProUGUI>().text = "FAIL!";

            timer.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "Time to beat:";
            timer.transform.Find("Message-Shadow").GetComponent<TextMeshProUGUI>().text = "Time to beat:";
        }

        float milliseconds = (float) TimeSpan.FromSeconds(PlayerSettings.Settings.fastestTime).Milliseconds;
        milliseconds /= 1000.0f;
        

        timer.transform.Find("Timer").GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}:{2:00.00}",
            Math.Truncate(TimeSpan.FromSeconds(PlayerSettings.Settings.fastestTime).TotalHours),
            Math.Truncate(TimeSpan.FromSeconds(PlayerSettings.Settings.fastestTime).TotalMinutes),
            (TimeSpan.FromSeconds(PlayerSettings.Settings.fastestTime).Seconds + milliseconds));

        timer.transform.Find("Timer-Shadow").GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}:{2:00.00}",
            Math.Truncate(TimeSpan.FromSeconds(PlayerSettings.Settings.fastestTime).TotalHours),
            Math.Truncate(TimeSpan.FromSeconds(PlayerSettings.Settings.fastestTime).TotalMinutes),
            (TimeSpan.FromSeconds(PlayerSettings.Settings.fastestTime).Seconds + milliseconds));

    }

    private void Transition()
    {
        currentTimeBetweenFrames += Time.deltaTime;
        screens[frameIndex].gameObject.SetActive(true);

        if (currentTimeBetweenFrames >= timeBetweenFrames)
        {

            if (frameIndex < screens.Count - 1)
            {
                frameIndex++;

                foreach (GameObject screen in screens)
                {
                    screen.gameObject.SetActive(false);
                }

                screens[frameIndex].gameObject.SetActive(true);
                currentTimeBetweenFrames = 0;
            }

            else if (frameIndex == screens.Count-1)
            {
                foreach (GameObject screen in screens)
                {
                    screen.gameObject.SetActive(false);
                }

                pause.paused = true;
            }  
        }
    }

    private void Update()
    {
        Comparison();
        Transition();
    }
}
