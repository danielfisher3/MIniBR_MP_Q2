
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

    [SerializeField] XRRayInteractor lHandRay;
    [SerializeField] LineRenderer lLineVisual;
    [SerializeField] XRInteractorLineVisual xrLLineVisual;




    private void Update()
    {


        RaycastResult resultR = new RaycastResult();
        if (rHandRay.TryGetCurrentUIRaycastResult(out resultR) == true)
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


        RaycastResult resultL = new RaycastResult();

        if(lHandRay.TryGetCurrentUIRaycastResult(out resultL) == true)
        {
            xrLLineVisual.enabled = true;
            rLineVisual.enabled = true;
        }
        else
        {
            if (xrLLineVisual.enabled && lLineVisual.enabled)
                xrLLineVisual.enabled = false;
                lLineVisual.enabled = false;
        }

    }
}