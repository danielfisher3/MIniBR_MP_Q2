/*Daniel Fisher
 * Aug 2022
 * Jammer Jamboree*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using BNG;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns in the network player and places them on the track
/// </summary>

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    

    public override void OnEnable()
    {
        base.OnEnable();
        //instantiate the player
            GameObject  player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position, Quaternion.identity, 0);
            
          
            //Get ref to the network player script on that prefab
            BNG.NetworkPlayer np = player.GetComponent<BNG.NetworkPlayer>();
            //if it isnt null
            if (np)
            {  
               
                //Setup remote player
                np.transform.name = "MyRemotePlayer";
                np.AssignPlayerObjects();
                
                
            }
           
        
        
        
    }
   
}
