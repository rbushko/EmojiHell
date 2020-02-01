using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiHelper : MonoBehaviour
{
    private EmojiType type;
    private List<string> selections;

    public void Init(EmojiType type, List<string> selections)
    {
        this.type = type;
        this.selections = selections;

        SetSprite();
    }

    private void SetSprite()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch(type)
        {
            case EmojiType.GOOD:
                spriteRenderer.sprite = AssetManager.GetEmoji(10);
                return;
            case EmojiType.BAD:
                spriteRenderer.sprite = AssetManager.GetEmoji(90);
                return;
            default:
                Debug.LogError("Emoji type was not good or bad");
                break;
        }
    }
}
