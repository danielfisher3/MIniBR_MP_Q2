using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActionBasedController))]
public class HandAnimations : MonoBehaviour
{
    ActionBasedController controller;
    [SerializeField] InputActionReference pistolDraw;
    public MyHand hand;

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

        if(pistolDraw.action.ReadValue<float>() > 0.01f && hand.hasPistol == false)
        {
            hand.hasPistol = true;
        }

        if(pistolDraw.action.ReadValue<float>() > 0.01f && hand.hasPistol == true)
        {
            hand.hasPistol = false;
        }
    }
}
