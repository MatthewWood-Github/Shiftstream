using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSettings : MonoBehaviour
{
    public PlayerMove player;
    public AIMove ai;
    public BaseMove preferredMovement;
    public BaseMove currentMovement { get; set; }

    public bool raceFinished { get; set; }

    public void toggleMovement()
    {
        if (currentMovement == player)
        {
            this.player.enabled = false;
            this.ai.enabled = true;
            currentMovement = this.ai;
        }
    }

    void Start()
    {
        this.player = this.GetComponent<PlayerMove>();
        this.ai = this.GetComponent<AIMove>();
        currentMovement = preferredMovement;
    }
}
