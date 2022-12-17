using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapCounter : MonoBehaviour
{
    [Header("Movement Components")]
    // Both scripts are required as the AI controls the player once the race is completed. 

    [Header("Lap Info")]
    /// <summary> The number of completed laps. </summary>
    [SerializeField] private int lapCount = 1;

    /// <summary> The maximum number of laps. </summary>
    [SerializeField] private int maxLaps = 3;

    [Header("Checkpoints")]
    /// <summary> The current number of checkpoints crossed in a lap. </summary>
    [SerializeField] private int checkpointCount;

    /// <summary> Parent object for checkpoints. </summary>
    [SerializeField] private Transform checkpointParent;

    /// <summary> List of checkpoints needed to cross to complete a lap. </summary>
    [SerializeField] private List<Transform> checkpoints;

    /// <summary> The maximum number of checkpoints. </summary>
    private int maxCheckpoints;

    /// <summary> The index of the next checkpoint in a lap. </summary>
    [SerializeField] private int targetCheckpoint = 1;

    [Header("SphereCast")]
    /// <summary> The radius of the SphereCast to detect checkpoint collisions. </summary>
    [SerializeField] private float raycastDistance = 10;

    private CarSettings carSettings;

    /// <summary> Checks for checkpoint collisions. </summary>
    /// <remarks> Uses sphere collider. </remarks>
    private void Checkpoint()
    {
        RaycastHit hit;

        if (Physics.SphereCast(this.transform.position, raycastDistance, transform.forward, out hit, raycastDistance))
        {
            if (hit.transform.tag == "Start" && checkpointCount == (maxCheckpoints - 1) && hit.transform.gameObject == checkpoints[targetCheckpoint].gameObject)
            {
                if (lapCount == maxLaps)
                {
                    carSettings.raceFinished = true;
                }
                else
                {
                    lapCount++;
                    targetCheckpoint++;
                    checkpointCount = 0;
                }
            }

            if (hit.transform.tag == "Checkpoint" && checkpoints[targetCheckpoint].gameObject == hit.transform.gameObject)
            {
                checkpointCount++;

                if (targetCheckpoint == maxCheckpoints - 1)
                    targetCheckpoint = 0;
                else targetCheckpoint++;
            }
        }
    }

    /// <summary> Returns the number of laps completed. </summary>
    /// <returns> The number of laps completed. </returns>
    public int GetLapCount()
    {
        return lapCount;
    }

    private void Start()
    {
        carSettings = this.GetComponent<CarSettings>();

        // Add all checkpoints to the checkpoints list.
        for (int x = 0; x < checkpointParent.childCount; x++)
            checkpoints.Add(checkpointParent.GetChild(x));

        maxCheckpoints = checkpoints.Count;
    }

    public void FixedUpdate()
    {
        Checkpoint();
    }
}
