using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    private static Dictionary<int, Sprite> emojis = new Dictionary<int, Sprite>();

    private static List<string> goodList = new List<string>();
    private static List<string> badList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        Sprite[] tempEmojis = Resources.LoadAll<Sprite>("Sprites/emojii_hell");
        foreach (Sprite s in tempEmojis)
        {
            int emojiNum = int.Parse(s.name.Substring(s.name.LastIndexOf("_") + 1));
            emojis.Add(emojiNum, s);
        }

        TextAsset tempList = Resources.LoadAll<TextAsset>("Lists/goodlist.txt");
        goodList = sourceTexts.text.Split("\n"[0]);

        tempList = Resources.LoadAll<TextAsset>("Lists/badlist.txt");
        badList = sourceTexts.text.Split("\n"[0]);
    }

    public static Sprite GetEmoji(int emojiNum)
    {
        return emojis[emojiNum];
    }
}
