using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControlManager : MonoBehaviour
{
    public GameObject joystickLeft;
    public GameObject joystickRight;



    private void Awake()
    {
#if UNITY_STANDALONE
        // Bilgisayarda çalışıyorsa joystick nesnesini gizle
        Destroy(joystickLeft);
        Destroy(joystickRight);
#elif UNITY_ANDROID
        // Android cihazda çalışıyorsa joystick nesnesini göster
        joystickLeft.SetActive(true);
        joystickRight.SetActive(true);
#endif
    }


}
