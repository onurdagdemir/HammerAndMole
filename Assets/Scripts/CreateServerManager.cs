using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateServerManager : MonoBehaviourPunCallbacks
{

    void Start()
    {
            PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Server Bağlantısı Başarılı");
        Debug.Log("Lobiye Bağlanılıyor");
        PhotonNetwork.NickName = PlayerPrefs.GetString("onlinePlayerName");
        PhotonNetwork.JoinLobby();
    }

}
