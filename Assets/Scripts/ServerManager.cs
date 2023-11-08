using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class ServerManager : MonoBehaviourPunCallbacks
{
    public Joystick joystickLeftRef;
    public Joystick joystickRightRef;
    public Transform target1Ref;
    public Transform target2Ref;
    public Transform target3Ref;
    public Transform target4Ref;
    public GameObject MoleOnline;
    public GameObject OnlineProblem;

    public GameObject pauseMenuRef;
    public Text moleCurrentHealthValueRef;
    public Text hammerCurrentHealthValueRef;
    public Text ResultTxt;
    public Text LobbyTxt;
    private int playerType;

    // Start is called before the first frame update
    void Awake()
    {
        playerType = PlayerPrefs.GetInt("playerType");

        if (PhotonNetwork.InRoom)
        {
            CreatePlayerCharacter();
        }
    }


    void CreatePlayerCharacter()
    {
        Debug.Log("Karakter Oluşturuluyor...");

        if (playerType == 0)
        {
            GameObject hammer = PhotonNetwork.Instantiate("HammerOnline", new Vector3(0.129f, 3.5f, -0.06f), Quaternion.identity, 0, null);
            NetworkAnimationTrigger hammerOnlineControl = hammer.GetComponent<NetworkAnimationTrigger>();
            if (hammerOnlineControl != null)
            {
                hammerOnlineControl.joystickLeft = joystickLeftRef;
                hammerOnlineControl.joystickRight = joystickRightRef;
            }

        }
        if (playerType == 1)
        {
            GameObject mole = PhotonNetwork.Instantiate("MoleOnline", new Vector3(2.34f, 1.05f, 1.47f), Quaternion.identity, 0, null);
            MoleOnlineControl moleOnlineControl = mole.GetComponent<MoleOnlineControl>();

            if (moleOnlineControl != null)
            {
                moleOnlineControl.joystickLeft = joystickLeftRef;
                moleOnlineControl.joystickRight = joystickRightRef;
                moleOnlineControl.moleHoles[0] = target1Ref;
                moleOnlineControl.moleHoles[1] = target2Ref;
                moleOnlineControl.moleHoles[2] = target3Ref;
                moleOnlineControl.moleHoles[3] = target4Ref;
            }

        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Bağlantı kesildi: " + cause.ToString());

        try
        {
            ResultTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("ConnectionProblem");
            LobbyTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("ReturnMainMenu");
            OnlineProblem.SetActive(true);
            PhotonNetwork.Disconnect();
            Invoke(nameof(TurnMainMenu), 3f);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Hata oluştu: " + e.Message);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        ResultTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("OtherPlayerLeft");
        LobbyTxt.text = Lean.Localization.LeanLocalization.GetTranslationText("ReturnLobby");
        OnlineProblem.SetActive(true);
        Invoke(nameof(TurnLobby), 3f);
    }

    private void TurnMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
    private void TurnLobby()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
