using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountDown : MonoBehaviour
{
    public Text countdownText;
    public float counter = 0f;
    private bool isUpdating = true;
    public GameObject pauseMenu;
    public AudioSource audioSource1; //3 ve 1 de ki ses
    public AudioSource audioSource2; // 2 deki ses
    public AudioSource audioSource3; // fight sesi
    private bool timeToStart = false;

    private bool isPlaySound1 = false;
    private bool isPlaySound2 = false;
    private bool isPlaySound3 = false;
    private bool isPlaySound4 = false;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("gameType") != 2)
        {
            Time.timeScale = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdating == true)
        {
            if (!pauseMenu.activeSelf)
            { 
            counter += Time.unscaledDeltaTime;
            }

            if (counter >= 6)
            {
                isUpdating = false;
                Destroy(gameObject);
            }

            else if (counter >= 5 && timeToStart == false)
            {
                Time.timeScale = 1;
                countdownText.text = "";
                timeToStart = true;
            }

            else if (counter >= 4 && timeToStart == false)
            {
                countdownText.text = Lean.Localization.LeanLocalization.GetTranslationText("Fight");
                PlaySound4();
            }

        else if(counter >= 3 && timeToStart == false)
            {
                countdownText.text = "1";
                PlaySound3();

            }
        else if (counter >= 2 && timeToStart == false)
            {
                countdownText.text = "2";
                PlaySound2();

            }
        else if (counter >= 0 && timeToStart == false)
            {
                countdownText.text = "3";
                PlaySound1();
            }


        }
       
    }

    // sesleri update içerisinde 1 kere çalması için

    private void PlaySound1()
    {
        if(isPlaySound1 == false)
        {
            audioSource1.Play();
            isPlaySound1 = true;
        }
    }

    private void PlaySound2()
    {
        if (isPlaySound2 == false)
        {
            audioSource2.Play();
            isPlaySound2 = true;
        }
    }

    private void PlaySound3()
    {
        if (isPlaySound3 == false)
        {
            audioSource1.Play();
            isPlaySound3 = true;
        }
    }

    private void PlaySound4()
    {
        if (isPlaySound4 == false)
        {
            audioSource3.Play();
            isPlaySound4 = true;
        }
    }

}
