using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "RHand")
        {
            other.gameObject.GetComponent<HandAnimationManager>().canHoldGun = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "RHand")
        {
            other.gameObject.GetComponent<HandAnimationManager>().canHoldGun = false;
        }
    }
}
