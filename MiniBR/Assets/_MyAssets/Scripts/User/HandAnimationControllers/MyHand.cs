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
    string gunHoldParam = "HasGun";
    string kParam = "HasKnife";
    string hhParam = "HasHH";
    public bool hasPistol;
    public bool hasMG;
    public bool hasKnife;
    public bool hasHandheld;
    public bool isLeft = false;
    bool hasGun;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject mg;

    // Start is called before the first frame update
    void Start()
    {
        handAnim = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        AnimateHand();
        if (hasPistol)
        {
            pistol.SetActive(true);
            mg.SetActive(false);
        }
        else
        {
            pistol.SetActive(false);
        }

        if (hasMG)
        {
            mg.SetActive(true);
            pistol.SetActive(false);
        }
        else
        {
            mg.SetActive(false);
        }
        
        if(hasMG || hasPistol)
        {
            hasGun = true;
        }
        else if(!hasMG || !hasPistol)
        {
            hasGun = false;
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
        handAnim.SetBool(gunHoldParam, hasGun);
        
        handAnim.SetBool(kParam, hasKnife);
        handAnim.SetBool(hhParam, hasHandheld);

        if (!isLeft)
        {
            if (!hasMG && !hasPistol)
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
        else
        {
            if (!hasKnife && !hasHandheld)
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
        
       
    }
}
