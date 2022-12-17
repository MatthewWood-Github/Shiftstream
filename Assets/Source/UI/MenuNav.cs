using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNav : MonoBehaviour
{
    public GameObject settings;
    public bool settingsActive = false;
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadTimeTrial()
    {
        SceneManager.LoadScene("Track1");
    }

    public void OpenSettings()
    {
        settingsActive = !settingsActive;
        settings.SetActive(settingsActive);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
    }
}
