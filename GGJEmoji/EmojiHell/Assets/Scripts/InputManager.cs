using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputManager : MonoBehaviour
{
    public TMP_InputField input;
    private List<string> selections;
    private string currentSelection = string.Empty;

    public void Init()
    {
        // input.onValueChanged += ValidateText;
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
        // Player hasn't started typing a valid word
        if (currentSelection.Equals(string.Empty))
        {
            // Find the word they are trying to type
            foreach (string selection in selections)
            {
                if (selection.IndexOf(input.text) >= 0)
                {
                    currentSelection = selection;
                    return;
                }
            }
        }
        // Validate that they are continuing to type the correct word
        else
        {
            if (currentSelection.IndexOf(input.text) == -1)
            {
                // Remove the last character they typed
                string currentText = input.text;
                input.text = currentText.Substring(0, currentText.Length - 1);
            }
        }
    }
}
