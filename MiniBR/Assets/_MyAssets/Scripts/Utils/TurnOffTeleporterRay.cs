using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TurnOffTeleporterRay : MonoBehaviour
{
    [SerializeField] XRRayInteractor lHandRay;
    [SerializeField] XRInteractorLineVisual lHandVisual;
    [SerializeField] LineRenderer lHandLRenderer;

   

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        if(lHandRay.TryGetCurrent3DRaycastHit(out hit))
        {
            if(hit.collider.gameObject.layer == 7)
            {
                lHandVisual.enabled = true;
                lHandVisual.enabled = true;
            }
            else
            {
                lHandVisual.enabled = false;
                lHandVisual.enabled = false;
            }
        }
        else
        {
            lHandVisual.enabled = false;
            lHandVisual.enabled = false;
        }

    }
}
