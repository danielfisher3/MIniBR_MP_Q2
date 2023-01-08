using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActionBasedController))]
public class HandAnimations : MonoBehaviour
{
    ActionBasedController controller;
    public InputActionReference pistolDraw;
    public InputActionReference mgDraw;
    public InputActionReference knifeDraw;
    public InputActionReference hhDraw;
    public MyHand hand;

    private void OnEnable()
    {
       
            pistolDraw.action.performed += ChanegPistolVar;
            mgDraw.action.performed += ChanegMGVar;
       
            knifeDraw.action.performed += ChanegKnifeVar;
            hhDraw.action.performed += ChanegHHVar;

        
    }
    private void OnDisable()
    {
       
            pistolDraw.action.canceled += ChanegPistolVar;
            mgDraw.action.canceled += ChanegMGVar;
       
        
       
            knifeDraw.action.canceled += ChanegKnifeVar;
            hhDraw.action.canceled += ChanegHHVar;
     
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        
            hand.SetGrip(controller.selectAction.action.ReadValue<float>());
            hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
        

       
    }

    void ChanegPistolVar(InputAction.CallbackContext context)
    {
        
            hand.hasPistol = !hand.hasPistol;
        if (hand.hasPistol)
        {
            hand.hasMG = false;
        }
        
    }
    void ChanegMGVar(InputAction.CallbackContext context)
    {
        
            hand.hasMG = !hand.hasMG;
        if (hand.hasMG)
        {
            hand.hasPistol = false;
        }
        
    }
    void ChanegKnifeVar(InputAction.CallbackContext context)
    {
       
            hand.hasKnife = !hand.hasKnife;
        if (hand.hasKnife)
        {
            hand.hasHandheld = false;
        }
        
    }
    void ChanegHHVar(InputAction.CallbackContext context)
    {
       
            hand.hasHandheld = !hand.hasHandheld;
        if (hand.hasHandheld)
        {
            hand.hasKnife = false;
        }
        
    }
}
