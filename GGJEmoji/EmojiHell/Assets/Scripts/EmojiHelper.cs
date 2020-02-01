using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiHelper : MonoBehaviour
{
    private EmojiType type;
    private List<string> selections;

    public SpriteRenderer sprite;

    public void Init(EmojiType type, List<string> selections)
    {
        this.type = type;
        this.selections = selections;

        SetSprite();
    }

    private void SetSprite()
    {
        switch(type)
        {
            case EmojiType.GOOD:
                // Set good emoji sprite
                return;
            case EmojiType.BAD:
                // Bad emoji sprite
                return;
            default:
                Debug.LogError("Emoji type was not good or bad");
                break;
        }
    }
}
