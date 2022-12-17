using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : BaseMove
{
    private void PlayerControls()
    {
        vertical = (int) Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButton("Drift") && movementSpeed > -1 && grounded && !isOffroad)
            drifting = true;   
        else drifting = false;

        if (Input.GetButton("Jump") && grounded)
            jumping = true;
        else jumping = false;

        if (Input.GetButton("Brake"))
            braking = true;
        else braking = false;
    }

    new private void Update()
    {
        base.Update();
        PlayerControls();
    }
 }
