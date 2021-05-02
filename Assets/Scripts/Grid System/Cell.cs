using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject Graphics;
    public GameObject Highlight;

    public bool IsActive = false;

    public void ToggleHighlight(bool toggle)
    {
        Highlight.SetActive(toggle);
    }

    public void ToggleGraphics(bool toggle)
    {
        Graphics.SetActive(toggle);
        IsActive = toggle;
    }
}
