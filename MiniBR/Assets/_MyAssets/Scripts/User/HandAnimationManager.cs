using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimationManager : MonoBehaviour
{
    [SerializeField] InputActionReference gripRef;
    [SerializeField]InputActionReference triggerRef;
    
    public float speed;
    Animator hAnimator;
    float gripTarget;
    float triggerTarget;
    float gripCurrent;
    float triggerCurrent;

    string gripP = "Grip";
    string triggerP = "Trigger";
    [SerializeField] bool isLeft;
    [SerializeField] bool hasGun = false;
    [SerializeField] GameObject handgun;


    // Start is called before the first frame update
    void Start()
    {
        hAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetGrip(gripRef.action.ReadValue<float>());
        SetTrigger(triggerRef.action.ReadValue<float>());
       

        AnimateHand();
        if (!isLeft)
        {
            if (!hasGun)
            {
                handgun.SetActive(false);
            }
            else if (hasGun && gripRef.action.ReadValue<float>() >= 0.25f)
            {
                handgun.SetActive(true);
            }
            else if (hasGun && gripRef.action.ReadValue<float>() < 0.25f)
            {
                handgun.SetActive(false);
            }
        }
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    void AnimateHand()
    {
        if (!isLeft)
        {
            hAnimator.SetBool("GunInHand", hasGun);
        }
        if(gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            hAnimator.SetFloat(gripP, gripCurrent);
        }

        if(triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            hAnimator.SetFloat(triggerP, triggerCurrent);
        }
    }
}
