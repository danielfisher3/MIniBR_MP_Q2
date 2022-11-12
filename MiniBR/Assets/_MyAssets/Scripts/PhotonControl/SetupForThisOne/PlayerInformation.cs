
using UnityEngine;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    public TMP_Text playerName;
    public Image playerBG;
    public Color highlightColor;
    Player player;

    

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
    }


   
    public void ApplyLocalPlayerHighlight()
    {
        playerBG.color = highlightColor;
    }
}
