/*Daniel Fisher
 * July 2022
 * Jammer Jamboree*/

using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/// <summary>
/// Manages lobby connection and room creation
/// shows other players in server/room
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Global Variables
    //room creation input field
    public TMP_InputField roomInputField;
    //lobby and room screens
    public GameObject lobbyScreen;
    public GameObject roomScreen;
    // room name ref
    public TMP_Text roomName;

    //item created for room entry
    public RoomItem roomItemPrefab;
    //how many rooms are there
    List<RoomItem> roomItemsList = new List<RoomItem>();
    //parent object created room items parent to
    public Transform roomParentObject;

    //cooldown for updates on player list and room list
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    //players in room that items have been created for
    List<PlayerItem> playerItemsList = new List<PlayerItem>();
    //player item for display
    public PlayerItem playerItemPrefab;
    //Transform to parent player items to
    public Transform playerItemParent;

    //ref to the "Start/Play" game button
    public GameObject playButton;
    //ref to change Track Button
    public GameObject changeTrackButton;

    #endregion


    #region Unity Native
    private void Start()
    {
        //make sure we have joined the main lobby
        PhotonNetwork.JoinLobby();
        changeTrackButton.SetActive(true);
    }

    private void Update()
    {
        //Check to see who is the master client as tehy will be the
       // only one able to see play button and start game
       //uncomment out the second half of if statement for builds or ready for nothing but multiplayer
        if(PhotonNetwork.IsMasterClient /*&& PhotonNetwork.CurrentRoom.PlayerCount >= 2*/)
        {
            playButton.SetActive(true);
            changeTrackButton.SetActive(true);
        }
        else 
        {
            playButton.SetActive(false);
            changeTrackButton.SetActive(false);
        }
    }
    #endregion


    #region Custom

    /// <summary>
    /// On a button click , a room will be created in photon
    /// </summary>
    public void OnClickCreateRoom()
    {
        //if name input has at least 1 char in it
        if (roomInputField.text.Length >= 1)
        {
            //tell photon to create the room
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 6 });
        }
    }


    /// <summary>
    /// On button click have photon load the level selected
    /// </summary>
    public void OnClickStartRace()
    {
        //PhotonNetwork.LoadLevel(UserGameCustomizer.instance.trackSelection +1);
    }


    /// <summary>
    /// On button click, leave current room
    /// </summary>
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    /// <summary>
    /// Joins a room, by name that photon has stored
    /// </summary>
    /// <param name="roomName"></param>
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }


    /// <summary>
    /// Updates the room list on screen for all
    /// </summary>
    /// <param name="list"></param>
    void UpdateRoomList(List<RoomInfo> list)
    {
        //first we have to destroy the rooms
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        //clear the list of rooms
        roomItemsList.Clear();

        //Update room list
        foreach (RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, roomParentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }



    /// <summary>
    /// Updates the player list on screen
    /// </summary>
    void UpdatePlayerList()
    {
        //destroy old list
        foreach (PlayerItem item in playerItemsList)
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
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalPlayerHighlight();
            }
            playerItemsList.Add(newPlayerItem);
        }
    }

    #endregion


    #region Photon Callbacks

    /// <summary>
    /// Double checks that we have joined the main lobby
    /// </summary>
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }


    /// <summary>
    /// Actions after we have joined a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        //change to room screen
        lobbyScreen.SetActive(false);
        roomScreen.SetActive(true);
        //set the room name text
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        //update players in room
        UpdatePlayerList();
        //for debugging
        print("Joined room: " + PhotonNetwork.CurrentRoom.Name);
    }


    /// <summary>
    /// Upadets rooom list in Photon
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }


    /// <summary>
    /// What happens when player enters room in photon
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }


    /// <summary>
    /// What happens when player leaves room in photon
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }


    /// <summary>
    /// More actions fotr after leaving room
    /// </summary>
    public override void OnLeftRoom()
    {
        //change back to lobby screen
        roomScreen.SetActive(false);
        lobbyScreen.SetActive(true);
        print("someone left the room");
    }
    #endregion
}
