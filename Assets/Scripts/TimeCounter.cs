using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{

    public delegate void TimeOverEventHandler();
    public static event TimeOverEventHandler OnTimeOver;

    public Text timerText;
    private float timeRemaining = 121f;
    private bool timerIsRunning = false;

    // Start is called before the first frame update

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("gameFrame"))
        {
            Application.targetFrameRate = 75;
        }
        else
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("gameFrame");
        }

    }
    void Start()
    {
        DisplayTime(timeRemaining);
        timerIsRunning = true;
        GameOverScript.OnGameOver += GameOver;

    }

    private void GameOver()
    {
        timerIsRunning = false;
    }

    private void OnDestroy()
    {
        GameOverScript.OnGameOver -= GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0.5f)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                // Zaman dolduğunda yapılacak işlemleri buraya ekleyebilirsiniz.
                timerIsRunning = false;
                OnTimeOver();
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        // Zamanı dakika:saniye formatında ekrana yazdırmak için
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
