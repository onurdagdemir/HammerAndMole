using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameOverScript : MonoBehaviourPunCallbacks
{

    public delegate void GameOverEventHandler();
    public static event GameOverEventHandler OnGameOver;

    [SerializeField]
    AudioClip buttonClickSoundEffect;

    [SerializeField]
    AudioClip blinkSoundEffect;

    public GameObject InfoScreenBackground;
    public Text resultText;

    public AudioSource audioSourceGameOverMenu;
    public GameObject MusicBackGround;

    private int isGameOver;

    public Button restartButton;
    public Text menuButtonText;
    private bool isPlayerRestart = false;
    private int gameType;

    public GameObject restartBlink;
    private bool isBlinkFlood = false;
    private int blinkFlood = 0;

    private void Start()
    {
        OnGameOver();
        MusicBackGround.SetActive(false);
        PlayGameOverMenuMusic();
        gameType = PlayerPrefs.GetInt("gameType");

        if(gameType == 2)
        {
            menuButtonText.text = "Lobby";
            // "Ready" tuşuna basıldığında oyuncu durumunu güncelle
            restartButton.onClick.AddListener(() =>
            {
                isPlayerRestart = !isPlayerRestart;
                if (isPlayerRestart)
                {
                    restartButton.GetComponentInChildren<Text>().text = "Undo";
                }
                else
                {
                    restartButton.GetComponentInChildren<Text>().text = "Restart";
                }

                // Oyuncunun hazır durumunu Photon ağına güncelle
                ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
                playerCustomProperties["IsRestart"] = isPlayerRestart;
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

                // Oyuncular hazır olduğunda oyunu başlat
                CheckPlayersRestart();
            });

            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties["IsRestart"] = isPlayerRestart;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

        }

    }

    //diğer oyuncudan restart talebi geldiğinde blink aktif eder
    private void toggleBlink(bool blinkIsOn)
    {
        if (!isPlayerRestart)
        {
            if (blinkIsOn)
            {
                restartBlink.SetActive(true);

                if (!isBlinkFlood && blinkFlood <= 5)
                {
                    AudioSource.PlayClipAtPoint(blinkSoundEffect, Camera.main.transform.position);
                    isBlinkFlood = true;
                    InfoScreenAction();
                    Invoke(nameof(blinkFloodControl), 2f);
                }
            }
            else if (!blinkIsOn)
            {
                restartBlink.SetActive(false);
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
            if (changedProps.ContainsKey("IsRestart"))
            {

                object isRestart;
                if (changedProps.TryGetValue("IsRestart", out isRestart))
                {
                    bool switchValue = (bool)isRestart;

                    toggleBlink(switchValue);
                }

                // Oyuncunun hazır durumunu kontrol et
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
            object isRestart;
            if (player.CustomProperties.TryGetValue("IsRestart", out isRestart))
            {
                if (!(bool)isRestart)
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
            isPlayerRestart = false;
            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties["IsRestart"] = isPlayerRestart;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
            PlayerPrefs.SetInt("isGameOver", 0);
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }

    public void Setup(string result)
    {
        gameObject.SetActive(true);
        resultText.text = result;
    }

    public void RestartButton()
    {
        if(gameType != 2)
        {
            AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
            Invoke(nameof(GameRestart), 0.5f);
        }

    }

    public void MenuButton()
    {
        if(gameType != 2)
        {
            Invoke(nameof(GoMenu), 0.5f);
        }
        else if(gameType == 2)
        {
            photonView.RPC("GoLobby", RpcTarget.Others);
            GoLobby();
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);

    }

    [PunRPC]
    private void GoLobby()
    {
        PlayerPrefs.SetInt("isGameOver", 0);
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    private void GameRestart()
    {
        PlayerPrefs.SetInt("isGameOver", 0);
        SceneManager.LoadScene("SampleScene");
    }

    private void GoMenu()
    {

        PlayerPrefs.SetInt("isGameOver", 0);
        SceneManager.LoadScene("StartScene");

    }

    private void PlayGameOverMenuMusic()
    {
        if(PlayerPrefs.GetInt("MusicOn") == 1)
        {
            audioSourceGameOverMenu.Play();
            audioSourceGameOverMenu.loop = true;
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
