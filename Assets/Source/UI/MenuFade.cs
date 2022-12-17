using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFade : MonoBehaviour
{
    public Image fade;
    public float faderate;

    private void Start()
    {
        fade = this.GetComponent<Image>();
    }
    void Update()
    {
        fade.color = new Color(0f, 0f, 0f, fade.color.a-faderate);
        if (fade.color.a <= faderate)
            fade.enabled = false;
    }
}
