using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    AudioClip buttonClickSoundEffect;

    public GameObject roomListScreen;
    public GameObject roomSettingsScreen;
    public GameObject roomListItemPrefab;
    public Transform roomListParent;
    private bool isOnlyAvailableRooms = false;
    public Button onlyAvbButton;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Start()
    {
        onlyAvbButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("ShowOnlyAvb");
        onlyAvbButton.onClick.AddListener(() =>
        {
                isOnlyAvailableRooms = !isOnlyAvailableRooms;
                if (isOnlyAvailableRooms)
                {

                    onlyAvbButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("ShowAllRooms");
            }
                else
                {

                    onlyAvbButton.GetComponentInChildren<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("ShowOnlyAvb");
            }


            UpdateRoomListUI();
        });
    }
    public void ClickSound()
    {
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public void OnRoomListButtonClicked()
    {
        roomListScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList = roomList;
        UpdateRoomListUI();
    }

    private void ClearRoomList()
    {
        foreach (Transform child in roomListParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateRoomListUI()
    {
        ClearRoomList();

        foreach (RoomInfo roomInfo in cachedRoomList)
        {
            if (isOnlyAvailableRooms)
            {
                if (!roomInfo.RemovedFromList && roomInfo.PlayerCount < roomInfo.MaxPlayers)
                {
                    GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);
                    RoomListItemUI roomListUI = roomItem.GetComponent<RoomListItemUI>();
                    if (roomItem != null)
                    {
                        roomListUI.Initialize(roomInfo);
                    }
                }
            }
            else
            {
                if (!roomInfo.RemovedFromList)
                {
                    GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);
                    RoomListItemUI roomListUI = roomItem.GetComponent<RoomListItemUI>();
                    if (roomItem != null)
                    {
                        roomListUI.Initialize(roomInfo);
                    }
                }
            }

            
        }

    }

    public void GoLobby()
    {
        roomListScreen.SetActive(false);
        roomSettingsScreen.SetActive(false);
        AudioSource.PlayClipAtPoint(buttonClickSoundEffect, Camera.main.transform.position);
    }
}
