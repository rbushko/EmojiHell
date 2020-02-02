using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiHelper : MonoBehaviour
{
    private EmojiType type;
    private List<string> selections;
    private int correctSelectionIndex;

    public void Init(EmojiType type, List<string> selections, Vector3 velocity, int correctSelectionIndex)
    {
        this.type = type;
        this.selections = selections;
        this.GetComponent<Rigidbody2D>().velocity = velocity;
        this.correctSelectionIndex = correctSelectionIndex;
        SetSprite();
    }

    private void SetSprite()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch(type)
        {
            case EmojiType.GOOD:
                spriteRenderer.sprite = AssetManager.GetGoodEmoji();
                return;
            case EmojiType.BAD:
                spriteRenderer.sprite = AssetManager.GetBadEmoji();
                return;
            case EmojiType.GOODOUT:
                spriteRenderer.sprite = AssetManager.GetEmoji(2);
                return;
            case EmojiType.BADOUT:
                spriteRenderer.sprite = AssetManager.GetEmoji(4);
                return;
            default:
                Debug.LogError("Emoji type was not good or bad");
                break;
        }
    }

    public EmojiType GetType()
    {
        return type;
    }

    public List<string> GetChoices()
    {
        return selections;
    }

    public int GetCorrectSelectionIndex()
    {
        return correctSelectionIndex;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    //set the emoji to not be active if it exits a despawn barrier
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "despawn")
        {
            //if the emoji overflowed, then penalize the player
            if (type == EmojiType.GOOD || type == EmojiType.BAD)
            {
                GameManager.g.UpdateScore(-1);
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "queue")
        {
            GameManager.g.AddEmoji(this);
        }
    }
}
