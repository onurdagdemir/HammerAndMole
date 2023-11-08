using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomListItemUI : MonoBehaviourPunCallbacks
{
    public Text roomNameText;
    public Text playerCountText;

    private string roomName;

    public RoomInfo RoomInfo { get; private set; }

    public void Initialize(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        roomName = roomInfo.Name;
        roomNameText.text = roomInfo.Name;
        playerCountText.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";

        Button joinButton = GetComponentInChildren<Button>();
        Image buttonImage = joinButton.GetComponent<Image>();

        if (roomInfo.PlayerCount == roomInfo.MaxPlayers)
        {
            joinButton.interactable = false;
            buttonImage.color = Color.red;
        }
        else
        {
            joinButton.interactable = true;
            buttonImage.color = Color.green;
        }

        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(OnJoinButtonClicked);
    }


    public void OnJoinButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            Invoke(nameof(JoinButtonAfter), 1f);
        }
    }
    private void JoinButtonAfter()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }
}
