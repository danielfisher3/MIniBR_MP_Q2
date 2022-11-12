
using UnityEngine;
using TMPro;

public class RoomInformation : MonoBehaviour
{
    public TMP_Text roomName;
    LobbyMaintainer lmanager;
    // Start is called before the first frame update
    void Start()
    {
        lmanager = FindObjectOfType<LobbyMaintainer>();

    }

   

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }


    /// <summary>
    /// On Button click join that room
    /// </summary>
    public void OnClickRoomItem()
    {
        lmanager.JoinRoom(roomName.text);


    }
}
