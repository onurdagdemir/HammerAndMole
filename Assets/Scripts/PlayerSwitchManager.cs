using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerSwitchManager : MonoBehaviourPunCallbacks
{
    public delegate void PlayerSwitchEventHandler();
    public static event PlayerSwitchEventHandler OnPlayerSwitch;

    [SerializeField]
    AudioClip buttonClickSoundEffect;

    [SerializeField]
    AudioClip blinkSoundEffect;

    public GameObject InfoScreenBackground;
    public Text infoTxt;
    public GameObject hammerLobby;
    public GameObject moleLobby;
    public TMPro.TMP_Text moleNameTxt;
    public TMPro.TMP_Text hammerNameTxt;

    private int playerType;
    private string playerName;
    private string otherPlayerName;

    public Button switchButton;

    private bool isPlayerSwitch = false;

    public GameObject blink;
    private bool isBlinkFlood = false;
    private int blinkFlood = 0;

    // Start is called before the first frame update
    void Start()
    {
        switchButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Switch");
        playerName = PlayerPrefs.GetString("onlinePlayerName");
        playerType = PlayerPrefs.GetInt("playerType");

        // "Switch" tuşuna basıldığında oyuncu durumunu güncelle
        switchButton.onClick.AddListener(() =>
        {
            Player[] players = PhotonNetwork.PlayerList;
            if (PhotonNetwork.InRoom && players.Length == 2)
            {
                isPlayerSwitch = !isPlayerSwitch;
                if (isPlayerSwitch)
                {
                    switchButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Undo");
                }
                else
                {
                    switchButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Switch");
                }
            }

            // Oyuncunun switch durumunu Photon ağına güncelle
            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties["IsSwitch"] = isPlayerSwitch;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

            // Oyuncular hazır olduğunda oyunu başlat
            CheckPlayersSwitch();
        });
    }
    //diğer oyuncudan switch talebi geldiğinde blink aktif eder
    private void toggleBlink(bool blinkIsOn)
    {
        if (!isPlayerSwitch)
        {
            if (blinkIsOn)
            {
                blink.SetActive(true);

                if (!isBlinkFlood && blinkFlood <= 5)
                {
                    AudioSource.PlayClipAtPoint(blinkSoundEffect, Camera.main.transform.position);
                    isBlinkFlood = true;
                    InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("OtherPlayerWantsToChange"));
                    Invoke(nameof(blinkFloodControl), 2f);
                }
            }
            else if (!blinkIsOn)
            {
                blink.SetActive(false);
            }
        }

    }

    private void blinkFloodControl()
    {
        isBlinkFlood = false;
        blinkFlood += 1;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        blinkFlood = 0;
    }

    // Oyuncunun özel özellikleri güncellendiğinde çağrılır
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Player[] players = PhotonNetwork.PlayerList;
        // Oyuncunun switch durumu güncellendiğinde
        if (changedProps.ContainsKey("IsSwitch") && players.Length == 2)
        {

            object isSwitch;
            if (changedProps.TryGetValue("IsSwitch", out isSwitch))
            {
                bool switchValue = (bool)isSwitch;

                toggleBlink(switchValue);
            }

            CheckPlayersSwitch();

        }
    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
        playerCustomProperties["IsSwitch"] = isPlayerSwitch;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

    }

    public void onSwitchPlayerPressed()
    {
        if (!PhotonNetwork.InRoom)
        {
            if (playerType == 0)
            {
                playerType = 1;
                PlayerPrefs.SetInt("playerType", playerType);
                hammerLobby.SetActive(false);
                moleLobby.SetActive(true);
                PlayerPrefs.SetString("moleOnlineName", playerName);
                moleNameTxt.text = playerName;
                hammerNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
                InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YourNewCharMole"));
            }
            else if (playerType == 1)
            {
                playerType = 0;
                PlayerPrefs.SetInt("playerType", playerType);
                hammerLobby.SetActive(true);
                moleLobby.SetActive(false);
                PlayerPrefs.SetString("hammerOnlineName", playerName);
                hammerNameTxt.text = playerName;
                moleNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
                InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YourNewCharHammer"));
            }


        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    private void CheckPlayersSwitch()
    {
        Player[] players = PhotonNetwork.PlayerList;
        bool allPlayersSwitch = true;

        foreach (Player player in players)
        {
            object isSwitch;
            if (player.CustomProperties.TryGetValue("IsSwitch", out isSwitch))
            {
                if (!(bool)isSwitch)
                {
                    allPlayersSwitch = false;
                    break;
                }
            }
            else
            {
                allPlayersSwitch = false;
                break;
            }
        }

        // Tüm oyuncular hazır durumdaysa switch çalıştır
        if (allPlayersSwitch && players.Length == 2)
        {
            isPlayerSwitch = false;
            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties["IsSwitch"] = isPlayerSwitch;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
            switchButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Switch");

            if (PhotonNetwork.IsMasterClient)
            {
                otherPlayerName = PlayerPrefs.GetString("otherPlayerName");

                if (playerType == 0)
                {
                    playerType = 1;
                    PlayerPrefs.SetInt("playerType", playerType);
                    PlayerPrefs.SetString("moleOnlineName", playerName);
                    PlayerPrefs.SetString("hammerOnlineName", otherPlayerName);
                    moleNameTxt.text = playerName;
                    hammerNameTxt.text = otherPlayerName;
                    InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YourNewCharMole"));
                }
                else if (playerType == 1)
                {
                    playerType = 0;
                    PlayerPrefs.SetInt("playerType", playerType);
                    PlayerPrefs.SetString("moleOnlineName", otherPlayerName);
                    PlayerPrefs.SetString("hammerOnlineName", playerName);
                    moleNameTxt.text = otherPlayerName;
                    hammerNameTxt.text = playerName;
                    InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YourNewCharHammer"));
                }

                OnPlayerSwitch();
            }

        }
        else if (players.Length == 1)
        {
            isPlayerSwitch = false;
            switchButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Switch");
            if (playerType == 0)
            {
                playerType = 1;
                PlayerPrefs.SetInt("playerType", playerType);
                hammerLobby.SetActive(false);
                moleLobby.SetActive(true);
                PlayerPrefs.SetString("moleOnlineName", playerName);
                moleNameTxt.text = playerName;
                hammerNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
                InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YourNewCharMole"));
            }
            else if (playerType == 1)
            {
                playerType = 0;
                PlayerPrefs.SetInt("playerType", playerType);
                hammerLobby.SetActive(true);
                moleLobby.SetActive(false);
                PlayerPrefs.SetString("hammerOnlineName", playerName);
                hammerNameTxt.text = playerName;
                moleNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
                InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YourNewCharHammer"));
            }

        }
    }

    private void InfoScreenAction(string info)
    {
        infoTxt.text = info;
        InfoScreenBackground.SetActive(true);
        CancelInvoke(nameof(InfoScreenClose));
        Invoke(nameof(InfoScreenClose), 2f);
    }
    private void InfoScreenClose()
    {
        InfoScreenBackground.SetActive(false);
    }
}
