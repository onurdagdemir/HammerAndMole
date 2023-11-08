using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    AudioClip buttonClickSoundEffect;

    [SerializeField]
    AudioClip blinkSoundEffect;

    public GameObject FirstTimeScreen;
    public GameObject CreateRoomScreen;
    public GameObject JoinRoomScreen;
    public GameObject LoadingScreen;
    public GameObject InfoScreenBackground;
    public Text infoTxt;
    public GameObject MainInfoScreenBackground;
    public Text mainInfoTxt;
    public GameObject roomListScreen;
    public GameObject SettingsScreen;
    public GameObject CreateServerManager;
    public GameObject hammerLobby;
    public GameObject moleLobby;
    public TMPro.TMP_Text moleNameTxt;
    public TMPro.TMP_Text hammerNameTxt;
    public InputField createRoomNameInput;
    public InputField joinRoomNameInput;
    private int playerType;
    public Text roomNameTxt;
    public Text statusTxt;
    private bool isConnectedToServer = false;

    public Button readyButton;

    private bool isPlayerReady = false;
    public GameObject readyBlink;
    public GameObject switchBlink;
    private bool isBlinkFlood = false;
    private int blinkFlood = 0;
    private string playerName;
    private string otherPlayerName;

    void Start()
    {
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll(PlayerPrefs.GetString("Language"));
        PlayerSwitchManager.OnPlayerSwitch += checkPlayerTypes;
        playerType = PlayerPrefs.GetInt("playerType");

        PlayerPrefs.SetInt("roundOnline", 0);
        PlayerPrefs.SetInt("wonOnline1", 0);
        PlayerPrefs.SetInt("wonOnline2", 0);

        playerName = PlayerPrefs.GetString("onlinePlayerName");

        if(PhotonNetwork.InLobby || PlayerPrefs.GetInt("OnlineConnection") == 0)
        {
            StartAction();
        }
        // Oyun bitişinde lobiye dönülürse çalışır
        else if (PhotonNetwork.InRoom)
        {
            if(PlayerPrefs.GetInt("isOtherLeftGame") == 1)
            {
                MainInfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("OtherPlayerLeftTheGame"));
            }
            playerType = PlayerPrefs.GetInt("playerType");
            roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
            statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForReady");
            PlayerPrefs.SetInt("isOtherLeftGame", 0);
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                hammerNameTxt.text = PlayerPrefs.GetString("hammerOnlineName");
                moleNameTxt.text = PlayerPrefs.GetString("moleOnlineName");
                hammerLobby.SetActive(true);
                moleLobby.SetActive(true);
            }       
            else
            {
                PhotonNetwork.JoinLobby();

                if (playerType == 1)
                {
                    moleNameTxt.text = PlayerPrefs.GetString("moleOnlineName");
                    moleLobby.SetActive(true);
                    hammerLobby.SetActive(false);
                    hammerNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
                }
                else if (playerType == 0)
                {
                    hammerNameTxt.text = PlayerPrefs.GetString("hammerOnlineName");
                    hammerLobby.SetActive(true);
                    moleLobby.SetActive(false);
                    moleNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
                }
            }
        }


        // "Ready" tuşuna basıldığında oyuncu durumunu güncelle
        readyButton.onClick.AddListener(() =>
        {
            isPlayerReady = !isPlayerReady;
            if (isPlayerReady)
            {
                readyButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Unready");
            }
            else
            {
                readyButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Ready");
            }

            // Oyuncunun hazır durumunu Photon ağına güncelle
            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties["IsReady"] = isPlayerReady;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

            // Oyuncular hazır olduğunda oyunu başlat
            CheckPlayersReady();
        });

    }

    private void OnDestroy()
    {
        PlayerSwitchManager.OnPlayerSwitch -= checkPlayerTypes;
    }

    //diğer oyuncudan ready talebi geldiğinde blink aktif eder
    private void toggleBlink(bool blinkIsOn)
    {
        if (!isPlayerReady)
        {
            if (blinkIsOn)
            {
                readyBlink.SetActive(true);

                if (!isBlinkFlood && blinkFlood <= 5)
                {
                    AudioSource.PlayClipAtPoint(blinkSoundEffect, Camera.main.transform.position);
                    isBlinkFlood = true;
                    isBlinkFlood = true;
                    InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("OtherPlayerWantsToPlay"));
                    Invoke(nameof(blinkFloodControl), 2f);
                }
            }
            else if (!blinkIsOn)
            {
                readyBlink.SetActive(false);
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
        // Oyuncunun hazır durumu güncellendiğinde
        if (changedProps.ContainsKey("IsReady"))
        {
            object isReady;
            if (changedProps.TryGetValue("IsReady", out isReady))
            {
                bool switchValue = (bool)isReady;

                toggleBlink(switchValue);
            }

            // Oyuncunun hazır durumunu kontrol et
            CheckPlayersReady();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (!isConnectedToServer)
        {
            InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("ServerConnectionSuccessful"));
            statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("ServerConnectionSuccessful");
            isConnectedToServer = true;
        }

        if(PlayerPrefs.GetInt("OnlineConnection") == 0)
        {
            FirstTimeScreen.SetActive(true);
            PlayerPrefs.SetInt("OnlineConnection", 1);
        }

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Bağlantı kesildi: " + cause.ToString());

        try
        {
            MainInfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("ConnectionProblem"));

            Invoke(nameof(ExitOnlineGame), 3f);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Hata oluştu: " + e.Message);
        }
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        roomNameTxt.text = "";
        statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForRoom");
        playerType = PlayerPrefs.GetInt("playerType");

        if (playerType == 1)
        {
            hammerNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
            hammerLobby.SetActive(false);
        }
        else if (playerType == 0)
        {
            moleNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
            moleLobby.SetActive(false);
        }
    }

    // Oyuncu odaya girdiğinde çağrılır
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Oyuncu Katıldı: " + newPlayer.NickName);
        otherPlayerName = newPlayer.NickName;
        PlayerPrefs.SetString("otherPlayerName", otherPlayerName);
        playerType = PlayerPrefs.GetInt("playerType");
        InfoScreenAction(otherPlayerName + Lean.Localization.LeanLocalization.GetTranslationText("HasJoined"));
        checkPlayerTypes();
        statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForReady");

        if (playerType == 1)
        {
            hammerNameTxt.text = otherPlayerName;
            PlayerPrefs.SetString("hammerOnlineName", otherPlayerName);
            hammerLobby.SetActive(true);
        }
        else if (playerType == 0)
        {
            moleNameTxt.text = newPlayer.NickName;
            PlayerPrefs.SetString("moleOnlineName", otherPlayerName);
            moleLobby.SetActive(true);
        }

        // Diğer oyuncunun ismini burada alabilirsiniz ve ekranda gösterebilirsiniz
    }

    private void checkPlayerTypes()
    {
        playerType = PlayerPrefs.GetInt("playerType");

        if (PhotonNetwork.IsMasterClient)
        {
            //Host Hammer ise diğer oyuncu da hammer ise mole' a çevir diğer oyuncuyu
            if (playerType == 0)
            {
                photonView.RPC("isHammer", RpcTarget.Others);
            }
            //Host Mole ise diğer oyuncu da mole ise hammer' a çevir diğer oyuncuyu
            else if (playerType == 1)
            {
                photonView.RPC("isMole", RpcTarget.Others);
            }

        }
    }

    //Alttaki 2 fonksiyon odaya join yapıldığında host ile karakter çakışmasını önlüyor
    [PunRPC]
    private void isHammer()
    {
        playerType = PlayerPrefs.GetInt("playerType");

        if (!PhotonNetwork.IsMasterClient)
        {
            otherPlayerName = PhotonNetwork.MasterClient.NickName;

            if (playerType == 0)
            {
                playerType = 1;
                PlayerPrefs.SetInt("playerType", playerType);
                hammerNameTxt.text = otherPlayerName;
                PlayerPrefs.SetString("hammerOnlineName", otherPlayerName);
                moleLobby.SetActive(true);
                moleNameTxt.text = playerName;
                PlayerPrefs.SetString("moleOnlineName", playerName);
                InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YourNewCharMole"));
            }
            else if(playerType == 1)
            {
                hammerNameTxt.text = otherPlayerName;
                PlayerPrefs.SetString("hammerOnlineName", otherPlayerName);
                hammerLobby.SetActive(true);
                moleNameTxt.text = playerName;
                PlayerPrefs.SetString("moleOnlineName", playerName);
            }
        }
    }

    [PunRPC]
    private void isMole()
    {
        playerType = PlayerPrefs.GetInt("playerType");

        if (!PhotonNetwork.IsMasterClient)
        {
            otherPlayerName = PhotonNetwork.MasterClient.NickName;

            if (playerType == 1)
            {
                playerType = 0;
                PlayerPrefs.SetInt("playerType", playerType);
                moleNameTxt.text = otherPlayerName;
                PlayerPrefs.SetString("moleOnlineName", otherPlayerName);
                hammerLobby.SetActive(true);
                hammerNameTxt.text = playerName;
                PlayerPrefs.SetString("hammerOnlineName", playerName);
                InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YourNewCharHammer"));
            }
            else if(playerType == 0)
            {
                moleNameTxt.text = otherPlayerName;
                PlayerPrefs.SetString("moleOnlineName", otherPlayerName);
                moleLobby.SetActive(true);
                hammerNameTxt.text = playerName;
                PlayerPrefs.SetString("hammerOnlineName", playerName);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomListScreen.SetActive(false);
        SettingsScreen.SetActive(false);
        roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
        InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YouJoinedTheRoom"));
        statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("YouJoinedTheRoom");

        // Oyuncu odaya girdiğine switch ve ready fonksiyonlarını host' a gönderir.
        ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
        playerCustomProperties["IsReady"] = isPlayerReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        blinkFlood = 0;
        readyBlink.SetActive(false);
        switchBlink.SetActive(false);
        InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("OtherPlayerLeftTheRoom"));
        statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("YouJoinedTheRoom");
        playerType = PlayerPrefs.GetInt("playerType");
        if (playerType == 1)
        {
            hammerNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer"); // İsim metnini boşaltarak silinmesini sağla
            hammerLobby.SetActive(false); // Karakteri deaktif hale getir
        }
        else if (playerType == 0)
        {
            moleNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer"); // İsim metnini boşaltarak silinmesini sağla
            moleLobby.SetActive(false); // Karakteri deaktif hale getir
        }
    }

    public void OnLeaveRoomButtonClicked()
    {
        // Odadan ayrıl
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("YouLeftTheRoom"));
            SettingsScreen.SetActive(false);
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }


    // Oyuncuların hazır durumunu kontrol et
    private void CheckPlayersReady()
    {
        Player[] players = PhotonNetwork.PlayerList;
        bool allPlayersReady = true;

        foreach (Player player in players)
        {
            object isReady;
            if (player.CustomProperties.TryGetValue("IsReady", out isReady))
            {
                if (!(bool)isReady)
                {
                    allPlayersReady = false;
                    break;
                }
            }
            else
            {
                allPlayersReady = false;
                break;
            }
        }

        // Tüm oyuncular hazır durumdaysa oyunu başlat
        if (allPlayersReady && players.Length == 2)
        {
            isPlayerReady = false;
            ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
            playerCustomProperties["IsReady"] = isPlayerReady;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);
            LoadingScreen.SetActive(true);
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }





    public void SettingsScreenOn()
    {
        SettingsScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void OnExitButtonPressed()
    {
        LoadingScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
        Invoke(nameof(ExitOnlineGame), 0.5f);
    }

    public void OnReadyButtonPressed()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    private void ExitOnlineGame()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("StartScene");
    }

    public void GoMenu()
    {
        SettingsScreen.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void CreateRoomPublic()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Invoke(nameof(CreateRoomPublicAfter), 1f);
        }
        else if (PhotonNetwork.InLobby)
        {
            if (createRoomNameInput.text != "")
            {
                PhotonNetwork.JoinOrCreateRoom(createRoomNameInput.text,
                new Photon.Realtime.RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true },
                Photon.Realtime.TypedLobby.Default);
                SettingsScreen.SetActive(false);
                CreateRoomScreen.SetActive(false);
            }
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    private void CreateRoomPublicAfter()
    {
        if (PhotonNetwork.InLobby)
        {
            if (createRoomNameInput.text != "")
            {
                PhotonNetwork.JoinOrCreateRoom(createRoomNameInput.text,
                new Photon.Realtime.RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true },
                Photon.Realtime.TypedLobby.Default);
                SettingsScreen.SetActive(false);
                CreateRoomScreen.SetActive(false);
            }
        }
    }

        public void CreateRoomPrivate()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Invoke(nameof(CreateRoomPrivateAfter), 1f);
        }
        else if (PhotonNetwork.InLobby)
        {
            if (createRoomNameInput.text != "")
            {
                PhotonNetwork.JoinOrCreateRoom(createRoomNameInput.text,
                new Photon.Realtime.RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = false },
                Photon.Realtime.TypedLobby.Default);
                SettingsScreen.SetActive(false);
                CreateRoomScreen.SetActive(false);
            }
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    private void CreateRoomPrivateAfter()
    {
        if (PhotonNetwork.InLobby)
        {
            if (createRoomNameInput.text != "")
            {
                PhotonNetwork.JoinOrCreateRoom(createRoomNameInput.text,
                new Photon.Realtime.RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = false },
                Photon.Realtime.TypedLobby.Default);
                SettingsScreen.SetActive(false);
                CreateRoomScreen.SetActive(false);
            }
        }
    }

        public void JoinRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Invoke(nameof(JoinRoomAfter), 1f);
        }
        else if (PhotonNetwork.InLobby)
        {
            if (joinRoomNameInput.text != "")
            {
                PhotonNetwork.JoinRoom(joinRoomNameInput.text);
                SettingsScreen.SetActive(false);
                JoinRoomScreen.SetActive(false);
            }
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    private void JoinRoomAfter()
    {
        if (PhotonNetwork.InLobby)
        {
            if (joinRoomNameInput.text != "")
            {
                PhotonNetwork.JoinRoom(joinRoomNameInput.text);
                SettingsScreen.SetActive(false);
                JoinRoomScreen.SetActive(false);
            }
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // Oda katılma başarısız olduğunda burası çalışır.
        Debug.LogError("Odaya katılma başarısız: " + message);
        MainInfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("TransactionFailed"));
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Odaya yaratma başarısız: " + message);
        MainInfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("TransactionFailed"));
        PhotonNetwork.JoinLobby();
    }

    // Photon sunucusuna bağlantı başarısız olduğunda bu metot çalışır.
    public void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        try
        {
            MainInfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("ConnectionProblem"));

            Invoke(nameof(ExitOnlineGame), 3f);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Hata oluştu: " + e.Message);
        }
        // Bağlantı başarısız olduğunda ekrana bir hata mesajı yazdırabilir veya diğer işlemleri gerçekleştirebilirsiniz.
        // Örneğin ana menüye dönmek gibi.
    }

    
    private void StartAction()
    {
            if (playerType == 0)
            {
                hammerNameTxt.text = playerName;
                PlayerPrefs.SetString("hammerOnlineName", playerName);
                hammerLobby.SetActive(true);
                moleLobby.SetActive(false);
                moleNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
            }
            else if (playerType == 1)
            {
                moleNameTxt.text = playerName;
                PlayerPrefs.SetString("moleOnlineName", playerName);
                hammerLobby.SetActive(false);
                moleLobby.SetActive(true);
                hammerNameTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("WaitingForOtherPlayer");
            }
            CreateServerManager.SetActive(true);
            statusTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("StatusFirst");

            infoTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("ConnectingToServer");
            InfoScreenBackground.SetActive(true);

    }



    // ILobbyCallbacks arayüzünden gelen odaları saklamak için bir liste
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    // ILobbyCallbacks.OnRoomListUpdate işlevini geçersiz kıl
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList = roomList;
    }

    public void onQuickMatchPressed()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Invoke(nameof(onQuickMatchPressedAfter), 1f);
        }
        else if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void onQuickMatchPressedAfter()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        InfoScreenAction(Lean.Localization.LeanLocalization.GetTranslationText("newRoomCreateAfterFail"));
        PhotonNetwork.JoinLobby();
        Invoke(nameof(createNewRoom), 2f);
    }

    [System.Obsolete]
    private void createNewRoom()
    {
        string newRoomName = "Room" + Random.RandomRange(1, 10000);
        PhotonNetwork.JoinOrCreateRoom(newRoomName,
        new Photon.Realtime.RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true },
        Photon.Realtime.TypedLobby.Default);
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

    private void MainInfoScreenAction(string info)
    {
        mainInfoTxt.text = info;
        MainInfoScreenBackground.SetActive(true);
        CancelInvoke(nameof(MainInfoScreenClose));
        Invoke(nameof(MainInfoScreenClose), 3f);
    }
    private void MainInfoScreenClose()
    {
        MainInfoScreenBackground.SetActive(false);
    }

    public void onCreateRoomScreen()
    {
        CreateRoomScreen.SetActive(true);
        SettingsScreen.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void onJoinRoomScreen()
    {
        JoinRoomScreen.SetActive(true);
        SettingsScreen.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void BackRoomScreens()
    {
        JoinRoomScreen.SetActive(false);
        CreateRoomScreen.SetActive(false);
        SettingsScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }


    public void BackFirstTimeScreen()
    {
        FirstTimeScreen.SetActive(false);
    }

}
