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
                //Set up the local player
                //LocalPlayer.lpInstance.gameObject.transform.position = player.transform.position;
              
                PlayerRotationPerTrackMultiAndLPSetup();
              
                //Setup remote player
                np.transform.name = "MyRemotePlayer";
                np.AssignPlayerObjects();
                
                
            }
           
        
        
        
    }
    void PlayerRotationPerTrackMultiAndLPSetup ()
    {
        /*if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Bristol"))
        {
            LocalPlayer.lpInstance.gameObject.transform.rotation = Quaternion.Euler(0, 0, 6);
            LPShutOffs();
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Darl"))
        {
            LocalPlayer.lpInstance.gameObject.transform.rotation = Quaternion.Euler(0, 97, 0);
            LPShutOffs();
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Dayton"))
        {
            LocalPlayer.lpInstance.gameObject.transform.rotation = Quaternion.Euler(0, 90, 14);
            LPShutOffs();
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("DG"))
        {
            LocalPlayer.lpInstance.gameObject.transform.rotation = Quaternion.Euler(0, 90, -3.75f);
            LPShutOffs();
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Dovery"))
        {
            LocalPlayer.lpInstance.gameObject.transform.rotation = Quaternion.Euler(0, 90, -10f);
            LPShutOffs();
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Martins"))
        {
            LocalPlayer.lpInstance.gameObject.transform.rotation = Quaternion.Euler(0, 90, 1.95f);
            LPShutOffs();
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("RichMoon"))
        {
            LocalPlayer.lpInstance.gameObject.transform.rotation = Quaternion.Euler(0, 90, 6f);
            LPShutOffs();
        }*/
    }
   
    /*void LPShutOffs()
    {
        LocalPlayer.lpInstance.lpJammer.SetActive(true);
        LocalPlayer.lpInstance.lpRGBY.isKinematic = true;
        LocalPlayer.lpInstance.lpRGBY.useGravity = false;
        LocalPlayer.lpInstance.vicController.enabled = true;
        LocalPlayer.lpInstance.SPJammerAttchmentChanger();
    }*/
}
