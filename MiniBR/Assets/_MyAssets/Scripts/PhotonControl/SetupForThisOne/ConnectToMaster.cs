
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ConnectToMaster : MonoBehaviourPunCallbacks

{
    public TMP_InputField userName;
    public TMP_Text buttonText;
    [SerializeField] UIMenuController uController;



    public void OnClickConnect()
    {
        if (userName.text.Length >= 1)
        {
            PhotonNetwork.NickName = userName.text;
            //Set text on Button for visual aid
            buttonText.text = "Connecting...";
            //Set sync to true to auto sync scenes across the network
            PhotonNetwork.AutomaticallySyncScene = true;
            //connect to main server using ther photon server settings in unity
            PhotonNetwork.ConnectUsingSettings();
        }
        
    }

    public override void OnConnectedToMaster()
    {

        uController.NextPage();

        //for debugging
        print("You are connected to main lobby!");
    }

    
}
