using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputManager : MonoBehaviour
{
    public TMP_InputField input;
    private List<string> selections;
    private string currentSelection = string.Empty;

    void Start()
    {
        // Temp starting selections
        selections = new List<string>();
        selections.Add("apple");
        selections.Add("bee");
        selections.Add("candle");
        selections.Add("dog");
    }

    public void NewInput(List<string> selections)
    {
        // Reset text input and currently typed word
        input.text = string.Empty;
        currentSelection = string.Empty;

        // Swap in new list of options to type
        this.selections = selections;
    }

    public void ValidateText(string newString)
    {
        Debug.LogError("Current selection is " + currentSelection);
        Debug.LogError("New string is " + newString);
        // Player hasn't started typing a valid word
        if (currentSelection.Equals(string.Empty))
        {
            // Find the word they are trying to type
            foreach (string selection in selections)
            {
                if (selection.IndexOf(newString) == 0)
                {
                    // If we have found the word they're typing,
                    // go ahead and return
                    currentSelection = selection;
                    return;
                }

                // If a word was not set, reset the text field
                input.text = string.Empty;
                currentSelection = string.Empty;
            }
        }
        // Validate that they are continuing to type the correct word
        else
        {
            if (currentSelection.IndexOf(newString) == -1)
            {
                // Remove the last character they typed
                input.text = newString.Substring(0, newString.Length - 1);
                Debug.LogError("Input is " + input.text);
            }
        }
    }

    public void OnSubmit(string finalString)
    {
        // Submit the score
        NewInput(selections);
    }
}
