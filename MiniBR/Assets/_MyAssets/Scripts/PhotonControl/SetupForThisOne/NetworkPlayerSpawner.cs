
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject networkPlayerPrefab;
    public Transform[] spawnPoints;

    public override void OnEnable()
    {
        base.OnEnable();
        //instantiate the player
        GameObject player = PhotonNetwork.Instantiate(networkPlayerPrefab.name, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position, Quaternion.identity, 0);


        //Get ref to the network player script on that prefab
        NetworkPlayerController np = player.GetComponent<NetworkPlayerController>();
        //if it isnt null
        if (np)
        {

            //Setup remote player
            np.transform.name = "MyRemotePlayer";
            np.AssignPlayerObjects();


        }

    }
}
