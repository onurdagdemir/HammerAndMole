using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerTouchControl : MonoBehaviour
{

    public delegate void HammerTouchEventHandler();
    public static event HammerTouchEventHandler upTouch;
    public static event HammerTouchEventHandler rightTouch;
    public static event HammerTouchEventHandler leftTouch;
    public static event HammerTouchEventHandler downTouch;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("playerType") == 1 && PlayerPrefs.GetInt("gameType") == 0)
        {
            Destroy(gameObject);
        }
    }
    public void upTouched()
    {
        upTouch();
    }
    public void rightTouched()
    {
        rightTouch();
    }
    public void leftTouched()
    {
        leftTouch();
    }
    public void downTouched()
    {
        downTouch();
    }
}
