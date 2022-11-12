/*Daniel Fisher
 * July 2022
 * Jammer Jamboree*/


using UnityEngine;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Representation of the Photon Player in the Ui menus
/// </summary>
public class PlayerItem : MonoBehaviour
{
    #region Global Variables
    
    public TMP_Text playerName;
    public Image playerBG;
    public Color highlightColor;
    Player player;
    #endregion

    #region Custom

    /// <summary>
    /// Sets the player info on screen 
    /// </summary>
    /// <param name="_player"></param>
    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
    }


    /// <summary>
    /// Highlights the local player player item for distinction
    /// </summary>
    public void ApplyLocalPlayerHighlight()
    {
        playerBG.color = highlightColor;
    }
    #endregion
}
