using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Checks independently if each wheel is grounded. </summary>
public class GroundCheck : MonoBehaviour
{
    /// <summary> The GameObjects representing the car's wheels. </summary>
    [SerializeField] private GameObject[] raycastPoints;

    /// <summary> Stores which wheels are grounded. </summary>
    [SerializeField] private bool[] groundedList = new bool[4];

    /// <summary> Stores the tag of the ground beneath each wheel. </summary>
    [SerializeField] private string[] tagList = new string[4];

    /// <summary> Checks independently if each wheel is grounded. </summary>
    /// <returns> True if any one wheel is grounded. </returns>
    public bool IsGrounded()
    {
        for (int x = 0; x < raycastPoints.Length; x++)
        {
            GameObject point = raycastPoints[x];
            groundedList[x] = point.GetComponent<Raycast>().GetGrounded();
        }

        if (groundedList.Contains(true))
            return true;
        else return false;
    }

    /// <summary> Checks independently if each wheel is offroad. </summary>
    /// <returns> True if any one wheel is offroad. </returns>
    public bool IsOffroad()
    {
        for (int x = 0; x < raycastPoints.Length; x++)
        {
            GameObject point = raycastPoints[x];
            tagList[x] = point.GetComponent<Raycast>().GetTag();
        }

        if (tagList.Contains("Offroad"))
            return true;
        else return false;

    }
}
