using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class XRKeyBoard : MonoBehaviour
{
    public TMP_InputField attachedInputField;

    public bool useShift = false;

    [Header("Sound FX")]
    public AudioClip keyPressSound;

    List<XRKey> keyboardKeys;

    private void Awake()
    {
        keyboardKeys = transform.GetComponentsInChildren<XRKey>().ToList();
    }

    public void PressKey(string key)
    {

        if (attachedInputField != null)
        {
            UpdateInputField(key);
        }
        else
        {
            Debug.Log("Pressed Key : " + key);
        }
    }

    public void UpdateInputField(string key)
    {
        string currentText = attachedInputField.text;
        int caretPosition = attachedInputField.caretPosition;
        int textLength = currentText.Length;
        bool caretAtEnd = attachedInputField.isFocused == false || caretPosition == textLength;

        // Formatted key based on short names
        string formattedKey = key;
        if (key.ToLower() == "space")
        {
            formattedKey = " ";
        }

        // Find KeyCode Sequence
        if (formattedKey.ToLower() == "backspace")
        {
            // At beginning of line - nothing to back into
            if (caretPosition == 0)
            {
                PlayClickSound(); // Still play the click sound
                return;
            }

            currentText = currentText.Remove(caretPosition - 1, 1);

            if (!caretAtEnd)
            {
                MoveCaretBack();
            }
        }
        else if (formattedKey.ToLower() == "enter")
        {
            // Debug.Log("Pressed Enter");
            // UnityEngine.EventSystems.ExecuteEvents.Execute(AttachedInputField.gameObject, null, UnityEngine.EventSystems.ExecuteEvents.submitHandler);
        }
        else if (formattedKey.ToLower() == "shift")
        {
            ToggleShift();
        }
        else
        {
            // Simply append the text to the end
            if (caretAtEnd)
            {
                currentText += formattedKey;
                MoveCaretUp();
            }
            else
            {
                // Otherwise we need to figure out how to insert the text and where
                string preText = "";
                if (caretPosition > 0)
                {
                    preText = currentText.Substring(0, caretPosition);
                }
                MoveCaretUp();

                string postText = currentText.Substring(caretPosition, textLength - preText.Length);

                currentText = preText + formattedKey + postText;
            }
        }

        // Apply the text change
        attachedInputField.text = currentText;

        PlayClickSound();

        // Keep Input Focused
        if (!attachedInputField.isFocused)
        {
            attachedInputField.Select();
        }
    }

    public virtual void PlayClickSound()
    {
        if (keyPressSound != null)
        {
            AudioSource.PlayClipAtPoint(keyPressSound, transform.position, 0.5f);
        }
    }

    public void MoveCaretUp()
    {
        StartCoroutine(IncreaseInputFieldCareteRoutine());
    }

    public void MoveCaretBack()
    {
        StartCoroutine(DecreaseInputFieldCareteRoutine());
    }

    public void ToggleShift()
    {
        useShift = !useShift;

        foreach (var key in keyboardKeys)
        {
            if (key != null)
            {
                key.ToggleShift();
            }
        }
    }
    IEnumerator IncreaseInputFieldCareteRoutine()
    {
        yield return new WaitForEndOfFrame();
        attachedInputField.caretPosition = attachedInputField.caretPosition + 1;
        attachedInputField.ForceLabelUpdate();
    }

    IEnumerator DecreaseInputFieldCareteRoutine()
    {
        yield return new WaitForEndOfFrame();
        attachedInputField.caretPosition = attachedInputField.caretPosition - 1;
        attachedInputField.ForceLabelUpdate();
    }

    public void AttachToInputField(TMP_InputField inputField)
    {
        attachedInputField = inputField;
    }
}
