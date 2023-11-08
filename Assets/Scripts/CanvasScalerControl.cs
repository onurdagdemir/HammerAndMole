using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerControl : MonoBehaviour
{

    public CanvasScaler canvasScaler;

    private void Start()
    {
        if (Screen.height >= 1150)
        {
            canvasScaler.scaleFactor = 1.3f;
        }
        else if (Screen.height > 1000)
        {
            canvasScaler.scaleFactor = 1f;
        }
        else if (Screen.height < 600)
        {
            canvasScaler.scaleFactor = 0.5f;
        }
        else
        {
            canvasScaler.scaleFactor = 0.7f;
        }
    }
}
