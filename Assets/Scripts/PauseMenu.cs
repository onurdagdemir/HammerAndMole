using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    public GameObject InfoScreenBackground;
    public GameObject pauseMenu;
    public Text timeCounterText;
    public GameObject musicBackgroundObject;
    public GameObject GameOverScreen;

    [SerializeField]
    AudioClip buttonClickSoundEffect;

    [SerializeField]
    AudioClip blinkSoundEffect;

    public bool isPaused;
    private bool isSoundOn;

    public Button restartButton;
    public Text menuButtonText;
    private bool isPlayerPauseRestart = false;
    private int gameType;

    public GameObject pauseRestartBlink;
    private bool isBlinkFlood = false;
    private int blinkFlood = 0;
    private bool exitGameByMe = false;

    // Start is called before the first frame update
    void Start()
    {
        gameType = PlayerPrefs.GetInt("gameType");
        pauseMenu.SetActive(false);
        musicBackgroundObject.SetActive(true);
        PlayerPrefs.SetInt("isGamePaused", 0);

        if (gameType == 2)
        {
            menuButtonText.text = "Lobby";
            // "Ready" tuşuna basıldığında oyuncu durumunu güncelle
            restartButton.onClick.AddListener(() =>
            {
                isPlayerPauseRestart = !isPlayerPauseRestart;
                if (isPlayerPauseRestart)
                {
                    restartButton.GetComponentInChildren<Text>().text = "Undo";
                }
                else
                {
                    restartButton.GetComponentInChildren<Text>().text = "Restart";
                }

                // Oyuncunun hazır durumunu Photon ağına güncelle
                ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
                playerCustomProperties["IsPauseRestart"] = isPlayerPauseRestart;
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

                // Oyuncular hazır olduğunda oyunu başlat
                CheckPlayersRestart();
            });

            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties["IsPauseRestart"] = isPlayerPauseRestart;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
        }
    }

    //diğer oyuncudan restart talebi geldiğinde blink aktif eder
    private void toggleBlink(bool blinkIsOn)
    {
        if (!isPlayerPauseRestart)
        {
            if (blinkIsOn)
            {
                pauseRestartBlink.SetActive(true);

                if(!isBlinkFlood && blinkFlood <= 5)
                {
                    AudioSource.PlayClipAtPoint(blinkSoundEffect, Camera.main.transform.position);
                    isBlinkFlood = true;
                    InfoScreenAction();
                    Invoke(nameof(blinkFloodControl), 2f);
                }
            }
            else if (!blinkIsOn)
            {
                pauseRestartBlink.SetActive(false);
            }
        }

    }

    private void blinkFloodControl()
    {
        isBlinkFlood = false;
        blinkFlood += 1;
    }

    // Oyuncunun özel özellikleri güncellendiğinde çağrılır
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (gameType == 2)
        {
            // Oyuncunun hazır durumu güncellendiğinde
            if (changedProps.ContainsKey("IsPauseRestart"))
            {
                object isPauseRestart;
                if (changedProps.TryGetValue("IsPauseRestart", out isPauseRestart))
                {
                    bool switchValue = (bool)isPauseRestart;
 
                    toggleBlink(switchValue);
                }

                // Oyuncunun restart durumunu kontrol et
                CheckPlayersRestart();
            }
        }

    }

    // Oyuncuların hazır durumunu kontrol et
    private void CheckPlayersRestart()
    {
        Player[] players = PhotonNetwork.PlayerList;
        bool allPlayersRestart = true;

        foreach (Player player in players)
        {
            object isPauseRestart;
            if (player.CustomProperties.TryGetValue("IsPauseRestart", out isPauseRestart))
            {
                if (!(bool)isPauseRestart)
                {
                    allPlayersRestart = false;
                    break;
                }
            }
            else
            {
                allPlayersRestart = false;
                break;
            }
        }

        // Tüm oyuncular hazır durumdaysa oyunu başlat
        if (allPlayersRestart)
        {
            isPlayerPauseRestart = false;
            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties["IsPauseRestart"] = isPlayerPauseRestart;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
            PlayerPrefs.SetInt("isGamePaused", 0);
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    // Update is called once per frame
    void Update()
    {


       if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseAciton();
        }

        if (isSoundOn == true)
        {

            AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
            isSoundOn = false;
        }

    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        if (gameType != 2)
        {
            Time.timeScale = 0;
        }
        isPaused = true;
        PlayerPrefs.SetInt("isGamePaused", 1);
    }

    public void GoMenu()
    {
        if (gameType != 2)
        {
            Time.timeScale = 1;
            Invoke(nameof(GameMenu), 0.5f);
        }
        else if (gameType == 2)
        {
            exitGameByMe = true;
            photonView.RPC("GoLobby", RpcTarget.Others);
            GoLobby();
        }
        isSoundOn = true;
        musicBackgroundObject.SetActive(false);
    }

    [PunRPC]
    private void GoLobby()
    {
        PlayerPrefs.SetInt("isGamePaused", 0);
        if (!exitGameByMe)
        {
            PlayerPrefs.SetInt("isOtherLeftGame", 1);
        }
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        if (gameType != 2)
        {
            if (timeCounterText.text == "")
            {
                Time.timeScale = 1;
            }
        }
        //isSounOn = true;  //resumegame sesi başlangıçta timescale 0 durumunda sıkıntılı
        isPaused = false;
        PlayerPrefs.SetInt("isGamePaused", 0);
    }

    public void GameRestart()
    {
        if (gameType != 2)
        {
            Time.timeScale = 1;
            Invoke(nameof(RestartScene), 0.5f);
            isSoundOn = true;
            musicBackgroundObject.SetActive(false);
        }
    }

    public void GameMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseAciton()
    {
        if (!GameOverScreen.activeSelf)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void InfoScreenAction()
    {
        InfoScreenBackground.SetActive(true);
        CancelInvoke(nameof(InfoScreenClose));
        Invoke(nameof(InfoScreenClose), 2f);
    }
    private void InfoScreenClose()
    {
        InfoScreenBackground.SetActive(false);
    }


}
