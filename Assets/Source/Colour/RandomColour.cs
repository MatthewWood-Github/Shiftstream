using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Assigns a random bright colour to this GameObject and applies the same colour to its trail. </summary>
public class RandomColour : MonoBehaviour
{
    /// <summary> Colour to assign the components. </summary>
    [SerializeField] private Color newColor;

    /// <summary> The renderer of this GameObject. </summary>
    private Renderer r;

    /// <summary> The trail of this GameObject. </summary>
    [SerializeField] private TrailRenderer trail;

    void Start()
    {
        newColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);

        trail.startColor = newColor;
        trail.endColor = newColor;

        r = this.transform.Find("Body").GetComponent<Renderer>();
        Material[] m = r.materials;
        
        foreach (var material in m)
        {
            if (material.name == "PlayerDefault (Instance)")
                material.color = newColor;
        }
    }
}
