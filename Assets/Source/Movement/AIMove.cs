using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : BaseMove
{
    private AINavigation nav;

    new private void Start()
    {
        base.Start();
        nav = this.GetComponent<AINavigation>();
    }
    new void Update()
    {
        base.Update();
        vertical = nav.Stop();
        horizontal = nav.SteerTowardsCheckpoint(); //+ nav.KeepInbounds();
        Debug.Log(nav.SteerTowardsCheckpoint());
    }
}
