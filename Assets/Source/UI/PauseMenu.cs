using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> Manages whether the game is in the paused state. </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary> Parent object of pause menu UI elements. </summary>
    [SerializeField] private GameObject pauseMenu;

    /// <summary> Parent object of all player HUD elements. </summary>
    [SerializeField] private GameObject playerUI;

    /// <summary> Is the game paused? </summary>
    public bool paused { get; set; }

    /// <summary> Decides which UI elements are displayed . </summary>
    /// <remarks> 
    /// If paused: the pause menu is active and the HUD is disabled. <br/>
    /// If not paused: the pause menu is disabled and the HUD is active.
    /// </remarks>
    private void PauseGame()
    {
        if (paused)
        {
            pauseMenu.SetActive(true);
            playerUI.SetActive(false);
        }
        else if (!paused)
        {
            pauseMenu.SetActive(false);
            playerUI.SetActive(true);
        }
    }

    /// <summary> Detects if the player has pressed the pause button. </summary>
    private void InputManager()
    {
        if (Input.GetButtonDown("Escape"))
            paused = !paused;
    }

    void Update()
    {
        InputManager();
        PauseGame();
    }
}
