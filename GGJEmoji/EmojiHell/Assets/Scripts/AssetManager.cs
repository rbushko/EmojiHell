using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    private static Dictionary<int, Sprite> emojis = new Dictionary<int, Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        Sprite[] tempEmojis = Resources.LoadAll<Sprite>("Sprites/emojii_hell");
        foreach (Sprite s in tempEmojis)
        {
            int emojiNum = int.Parse(s.name.Substring(s.name.LastIndexOf("_") + 1));
            emojis.Add(emojiNum, s);
        }
    }

    public static Sprite GetEmoji(int emojiNum)
    {
        return emojis[emojiNum];
    }
}
