using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyHand : MonoBehaviour
{
    Animator handAnim;

    float griptarget;
    float triggerTarget;
    float currentTriggerValue;
    float currentGripValue;
    [SerializeField]float speed;
    string animatorGripParam = "Grip";
    string animatorTriggerParam = "Trigger";
    string gunParam = "GunInHand";
    public bool hasPistol;
    [SerializeField] GameObject pistol;
    // Start is called before the first frame update
    void Start()
    {
        handAnim = GetComponent<Animator>();
        hasPistol = false;
        if (pistol != null)
        {
            pistol.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handAnim.SetBool(gunParam, hasPistol);
        if (!hasPistol)
        {
            AnimateHand();
            if(pistol != null)
            {
                pistol.SetActive(false);
            }
        }
        else
        {
            pistol.SetActive(true);
        }
        
    }

    internal void SetGrip(float v)
    {
        griptarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    
    void AnimateHand()
    {
        
            
            
            if (currentGripValue != griptarget)
            {
                currentGripValue = Mathf.MoveTowards(currentGripValue, griptarget, Time.deltaTime * speed);
                handAnim.SetFloat(animatorGripParam, currentGripValue);
            }

            if (currentTriggerValue != triggerTarget)
            {
                currentTriggerValue = Mathf.MoveTowards(currentTriggerValue, triggerTarget, Time.deltaTime * speed);
                handAnim.SetFloat(animatorTriggerParam, currentTriggerValue);
            }
        
       
    }
}
