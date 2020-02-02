using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    private static Dictionary<int, Sprite> emojis = new Dictionary<int, Sprite>();
    private static List<string> goodList;
    private static List<string> badList;
    private static System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        Sprite[] tempEmojis = Resources.LoadAll<Sprite>("Sprites/emojii_hell");
        foreach (Sprite s in tempEmojis)
        {
            int emojiNum = int.Parse(s.name.Substring(s.name.LastIndexOf("_") + 1));
            emojis.Add(emojiNum, s);
        }

        rand = new System.Random();

        TextAsset tempList = Resources.Load<TextAsset>("Lists/goodlist");
        goodList = new List<string>(tempList.text.Split("\n"[0]));

        tempList = Resources.Load<TextAsset>("Lists/badlist");
        badList = new List<string>(tempList.text.Split("\n"[0]));
    }

    public static Sprite GetEmoji(int emojiNum)
    {
        return emojis[emojiNum];
    }

    //gets the angel emoji
    public static Sprite GetGoodEmoji()
    {
        return emojis[10];
    }
    //gets the devil emoji
    public static Sprite GetBadEmoji()
    {
        return emojis[90];
    }

    public static List<string> GetGoodList()
    {
        return goodList;
    }

    public static List<string> GetBadList()
    {
        return badList;
    }

    public static string GetRandomGoodItem()
    {
        return goodList[rand.Next(goodList.Count)];
    }

    public static string GetRandomBadItem()
    {
        return badList[rand.Next(badList.Count)];
    }
}
