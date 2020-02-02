using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI; 

public class TextOptionHelper : MonoBehaviour
{
    public TextMeshProUGUI text;
    private string color1 = "<color=\"red\">";
    private string color2 = "<color=\"white\">";
    private string curString;
    private int position;

    public void init(string s)
    {
        curString = s;
        text.text = color2 + curString;
        position = 1;
    }

    public void advanceNextChar()
    {
        text.text = color1 + curString.Substring(0, position) + color2 + curString.Substring(position);
        if (position != curString.Length)
        {
            position++;
        }
    }

    public string nextChar()
    {
        return curString[position-1] + "";
    }

    public bool isComplete()
    {
        return position == curString.Length;
    }
}
