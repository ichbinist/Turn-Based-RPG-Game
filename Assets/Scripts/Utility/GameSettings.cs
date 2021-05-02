using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int TargetFrameRate = 30;

    private void Start()
    {
        Application.targetFrameRate = TargetFrameRate;
    }
}
