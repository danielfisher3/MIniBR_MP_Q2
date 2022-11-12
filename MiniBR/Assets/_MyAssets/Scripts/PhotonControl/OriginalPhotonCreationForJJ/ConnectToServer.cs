/*Daniel Fisher
 * July 2022
 * Jammer Jamboree*/

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/// <summary>
/// Connects to main Photon Server
/// </summary>
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    #region Global Variables
    public TMP_InputField userNameInput;
    public TMP_Text buttonText;
    public GameObject lobbyPage;
    public GameObject multiNamePage;
   
    #endregion

    #region Custom

    /// <summary>
    /// Connects to the main Photon Server from button click
    /// </summary>
    public void OnClickConnect()
    {
        //check if player has input something for their name
        if(userNameInput.text.Length >= 1)
        {
            //set it to Photon NickName
            PhotonNetwork.NickName = userNameInput.text;
            //Set text on Button for visual aid
            buttonText.text = "Connecting...";
            //Set sync to true to auto sync scenes across the network
            PhotonNetwork.AutomaticallySyncScene = true;
            //connect to main server using ther photon server settings in unity
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion

    #region Photon Callbacks
    /// <summary>
    /// Callback that refs when we connect to master server
    /// </summary>
    public override void OnConnectedToMaster()
    {
        //change from name entry to lobby page when connected
        multiNamePage.SetActive(false);
        lobbyPage.SetActive(true);
      
        //for debugging
        print("You are connected to main lobby!");
    }
    #endregion
}
