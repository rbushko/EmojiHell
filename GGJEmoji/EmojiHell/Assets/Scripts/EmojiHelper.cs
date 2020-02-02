using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiHelper : MonoBehaviour
{
    private EmojiType type;
    private List<string> selections;

    public void Init(EmojiType type, List<string> selections, Vector3 velocity)
    {
        this.type = type;
        this.selections = selections;
        this.GetComponent<Rigidbody2D>().velocity = velocity;
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

    //set the emoji to not be active if it exits a despawn barrier
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "despawn")
        {
            Destroy(gameObject);
        }
    }
}
