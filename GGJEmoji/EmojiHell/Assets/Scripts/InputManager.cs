using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputField input;
    private List<string> selections;

    public void NewInput(List<string> selections)
    {
        // input.
        this.selections = selections;
    }

    
}
