/*Daniel Fisher
 * July 2022
 * Jammer Jamboree*/

using UnityEngine;
using TMPro;


/// <summary>
/// Sets room item info
/// </summary>
public class RoomItem : MonoBehaviour
{
    #region Global Variables
    public TMP_Text roomName;
    LobbyManager lmanager;
    #endregion

    #region Unity Native
    void Start()
    {
        lmanager = FindObjectOfType<LobbyManager>();
    }
    #endregion

    #region Custom
    /// <summary>
    /// Set Room Name
    /// </summary>
    /// <param name="_roomName"></param>
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
    #endregion
}
