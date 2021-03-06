﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InputManager : MonoBehaviour
{
    public TMP_InputField input;
    private List<TextOptionHelper> selections;
    private int selectionIndex = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (!input.isFocused)
        {
            EventSystem.current.SetSelectedGameObject(input.gameObject, null);
            input.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }

    public void GetInput(string newInput)
    {
        GameManager.g.CheckInput(newInput);
        input.text = string.Empty;
        
    }

    public void NewInput(string selections)
    {
        // Reset text input and currently typed word
        input.text = string.Empty;
        selectionIndex = -1;

        // Swap in new list of options to type
        //this.selections = selections;
    }

    public void ValidateText(string newString)
    {
        // Player hasn't started typing a valid word
        if (selectionIndex < 0)
        {
            // Find the word they are trying to type
            for (int i = 0; i < selections.Count; i++)
            {
                if (selections[i].nextChar() == newString)
                {
                    // If we have found the word they're typing,
                    // go ahead and return
                    selectionIndex = i;
                    return;
                }

                // If a word was not set, reset the text field
                input.text = string.Empty;
                selectionIndex = -1;
            }
        }
        // Validate that they are continuing to type the correct word
        else
        {
            string currentSelection = selections[selectionIndex].text.text;
            if (currentSelection.IndexOf(newString) == -1)
            {
                // Remove the last character they typed
                input.text = newString.Substring(0, newString.Length - 1);
            }
        }
    }
    
}
