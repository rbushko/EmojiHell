using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmojiFactory : MonoBehaviour
{
    public GameObject cloneMoji;

    private List<string> goodChoices = new List<string>();
    private List<string> badChoices = new List<string>();

    public void MakeEmoji()
    {
        System.Random rand = new System.Random();
        
        EmojiType type = (EmojiType) rand.Next(2);
        Debug.LogError("Type: " + type);

        List<string> choices = GetChoiceList(type);

        GameObject newEmoji = Instantiate(cloneMoji);
        newEmoji.GetComponent<EmojiHelper>().Init(type, choices);
    }

    private List<string> GetChoiceList(EmojiType type)
    {
        List<string> choices = new List<string>();
        List<string> otherOptions;
        
        System.Random rand = new System.Random();
        if (type == EmojiType.GOOD)
        {
            choices.Add(goodChoices[rand.Next(goodChoices.Count)]);
            otherOptions = badChoices;
        }
        else
        {
            choices.Add(badChoices[rand.Next(badChoices.Count)]);
            otherOptions = goodChoices;
        }

        
        // Create a list of 4 choices with unique first letters
        while (choices.Count < 4)
        {
            bool addPossible = true;
            string possibleOption = otherOptions[rand.Next(otherOptions.Count)];
            foreach (string choice in choices)
            {
                // If they have the same first character, don't add it
                if (choice[0] == possibleOption[0])
                {
                    addPossible = false;
                }
            }

            if (addPossible)
            {
                choices.Add(possibleOption);
            }
        }

        return choices;
    }
}
