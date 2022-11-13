
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TurnOffHandRaysUI : MonoBehaviour
{
    
    [SerializeField] XRRayInteractor rHandRay;
    [SerializeField] LineRenderer rLineVisual;
    [SerializeField] XRInteractorLineVisual xrRLineVisual;
    




   
    private void Update()
    {


        RaycastResult result = new RaycastResult();
        if (rHandRay.TryGetCurrentUIRaycastResult(out result) == true)
        {
            xrRLineVisual.enabled = true;
            rLineVisual.enabled = true;
        }
        else
        {
            if(xrRLineVisual.enabled && rLineVisual.enabled)
                xrRLineVisual.enabled = false;
                rLineVisual.enabled = false;
        }

    }
}