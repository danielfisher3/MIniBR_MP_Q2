using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class XRKey : MonoBehaviour
{
     Button thisButton;
     TMP_Text buttonText;

     XRKeyBoard xrKeyBoard;

    public string keyCode;

    public string keyCodeShift;

    [HideInInspector]
    public bool useShift = false;



    private void Awake()
    {
        thisButton = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();

        if(thisButton != null)
        {
            thisButton.onClick.AddListener(OnKeyHit);
        }

         xrKeyBoard = GetComponentInParent<XRKeyBoard>();
    }
    
    public virtual void ToggleShift()
    {
        useShift = !useShift;

        // Make sure the button exists
        if (buttonText == null)
        {
            return;
        }

        // Update text label
        if (useShift && !string.IsNullOrEmpty(keyCodeShift))
        {
            buttonText.text = keyCodeShift;
        }
        else
        {
            buttonText.text = keyCode;
        }
    }

    public virtual void OnKeyHit()
    {
        OnKeyHit(useShift && !string.IsNullOrEmpty(keyCodeShift) ? keyCodeShift : keyCode);
    }

    public virtual void OnKeyHit(string key)
    {
        if (xrKeyBoard != null)
        {
            xrKeyBoard.PressKey(key);
        }
        else
        {
            Debug.Log("Pressed key " + key + ", but no keyboard was found");
        }
    }
}
