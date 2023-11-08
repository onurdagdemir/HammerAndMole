using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterTest : MonoBehaviour
{
    public GameObject TestButton1;
    public GameObject TestButton2;
    public GameObject TestButton3;
    public GameObject InputField;
    public GameObject EnterButton;
    public InputField PasswordInput;

    private int buttonPressCount = 0;

    public void buttonPressed()
    {
        buttonPressCount += 1;
        if(buttonPressCount >= 15)
        {
            InputField.SetActive(true);
            EnterButton.SetActive(true);
        }
    }

    public void enterMaster()
    {
        if(PasswordInput.text == "Time2042!")
        {
            TestButton1.SetActive(true);
            TestButton2.SetActive(true);
            TestButton3.SetActive(true);
        } else
        {
            PasswordInput.text = "Parola Geçersiz!";
        }
    }
}
