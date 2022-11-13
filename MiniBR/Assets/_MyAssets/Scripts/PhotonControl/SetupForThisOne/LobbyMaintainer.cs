
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyMaintainer : MonoBehaviourPunCallbacks

{
    [SerializeField] UIMenuController uiController;
    //room creation input field
    public TMP_InputField roomInputField;

    // room name ref
    public TMP_Text roomName;

    //item created for room entry
    public RoomInformation roomItemPrefab;
    //how many rooms are there
    List<RoomInformation> roomItemsList = new List<RoomInformation>();
    //parent object created room items parent to
    public Transform roomParentObject;

    //cooldown for updates on player list and room list
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    //players in room that items have been created for
    List<PlayerInformation> playerItemsList = new List<PlayerInformation>();
    //player item for display
    public PlayerInformation playerItemPrefab;
    //Transform to parent player items to
    public Transform playerItemParent;

    //ref to the "Start/Play" game button
    public GameObject playButton;


    void Start()
    {
        //make sure we have joined the main lobby
        PhotonNetwork.JoinLobby();
    }

    

    public void OnClickCreateRoom()
    {
        //if name input has at least 1 char in it
        if (roomInputField.text.Length >= 1)
        {
            //tell photon to create the room
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 6 });
        }
    }


    public void OnClickStartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        //first we have to destroy the rooms
        foreach (RoomInformation item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        //clear the list of rooms
        roomItemsList.Clear();

        //Update room list
        foreach (RoomInfo room in list)
        {
            RoomInformation newRoom = Instantiate(roomItemPrefab, roomParentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    void UpdatePlayerList()
    {
        //destroy old list
        foreach (PlayerInformation item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        //clear old list
        playerItemsList.Clear();

        //check to make sure we are in a room witrh players
        if (PhotonNetwork.CurrentRoom == null) return;

        //repopulate list
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerInformation newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalPlayerHighlight();
            }
            playerItemsList.Add(newPlayerItem);
        }
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        //change to room screen
        uiController.NextPage();
        //set the room name text
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        //update players in room
        UpdatePlayerList();
        //for debugging
        print("Joined room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }


    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnLeftRoom()
    {
        uiController.PrevPage();
        print("someone left the room");
    }
}


