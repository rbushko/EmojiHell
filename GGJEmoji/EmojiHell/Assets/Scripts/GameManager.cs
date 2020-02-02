using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager g;

    //time per game
    public int timePerGame;
    private int timeLeft;

    //items that need activated/deactivated on game start/end
    public GameObject startScreen;
    public GameObject gameScreen;
    public GameObject gameObjects;
    public GameObject endScreen;


    //the particle effects and sound effects
    public List<GameObject> particleEffects;
    public List<AudioClip> audioClips;
    private AudioSource audioPlayer;

    //to keep track of the emojis that need taken care of
    private Queue<EmojiHelper> emojis = new Queue<EmojiHelper>();
    private EmojiHelper curEmoji = null;

    //trackers
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    public EmojiFactory factory;

    //to show the emoji stats
    public List<TextOptionHelper> textAreas;
    private List<string> nextChars;
    public Image curEmojiImage;

    private static int score;

    void Start()
    {
        g = this;
        nextChars = new List<string>();
        nextChars.Add(" ");
        nextChars.Add(" ");
        nextChars.Add(" ");
        nextChars.Add(" ");
        timeLeft = timePerGame;
        score = 0;
    }

    public void UpdateScore(int toAdd)
    {
        score+= toAdd;
        scoreText.text = "Score: " + score;
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
        endScreen.SetActive(false);
        gameScreen.SetActive(true);
        gameObjects.SetActive(true);
        timeLeft = timePerGame;
        UpdateScore(-score);
        StartCoroutine("Timer");
    }

    public void GameOver()
    {
        endScreen.SetActive(true);
        gameScreen.SetActive(false);
        gameObjects.SetActive(false);
        StopCoroutine("Timer");
    }

    IEnumerator Timer()
    {
        for(;;)
        {
            timerText.text = "Time Left: " + timeLeft;
            if (timeLeft % 5 == 0)
            {
                factory.MakeEmojiInput();
            }
            yield return new WaitForSeconds(1.0f);
            timeLeft--;
            if (timeLeft < 0)
            {
                GameOver();
            }
        }
    }

#region emojis
    public void AddEmoji(EmojiHelper newEmoji)
    {
        if (curEmoji == null)
        {
            SetUpForNewEmoji(newEmoji);
        }
        else
        {
            emojis.Enqueue(newEmoji);
        }
    }

    public void NextEmoji()
    {
        EmojiHelper oldEmoji = curEmoji;
        oldEmoji.Destroy();
        if (emojis.Count > 0)
        {
            SetUpForNewEmoji(emojis.Dequeue());
        }
        else
        {
            curEmoji = null;
            factory.MakeEmojiInput();
        }
    }

    private void SetUpForNewEmoji(EmojiHelper newEmoji)
    {
        curEmoji = newEmoji;
        //taking care of the "current emoji" sprite
        if (newEmoji.GetType() == EmojiType.GOOD)
        {
            curEmojiImage.sprite = AssetManager.GetGoodEmoji();
        }
        else
        {
            curEmojiImage.sprite = AssetManager.GetBadEmoji();
        }

        //updating the selections
        int i = 0;
        foreach(string s in newEmoji.GetChoices())
        {
            textAreas[i].init(s);
            nextChars[i] = textAreas[i].nextChar();
            i++;
        }
    }
#endregion emojis

    public void CheckInput(string s)
    {
        //Debug.Log("got: " + s + " should be: " + nextChars[0]);
        for(int i = 0; i < textAreas.Count; i++)
        {
            if (nextChars[i] == s)
            {
                textAreas[i].advanceNextChar();
                nextChars[i] = textAreas[i].nextChar();
                if (textAreas[i].isComplete())
                {
                    FinishInput(i);
                }
            }
        }
    }

    //takes in the index that the words were finished on and checks if it is right
    private void FinishInput(int finalIndex)
    {
        if (finalIndex == curEmoji.GetCorrectSelectionIndex())
        {
            //Debug.Log("Correct!");
            UpdateScore(10);
            if (curEmoji.GetType() == EmojiType.GOOD)
            {
                factory.MakeEmojiOutput(1);
            }
            else
            {
                factory.MakeEmojiOutput(0);
            }
        }
        else
        {
            //Debug.Log("Nope!");
            UpdateScore(-1);
            if (curEmoji.GetType() == EmojiType.GOOD)
            {
                factory.MakeEmojiOutput(0);
            }
            else
            {
                factory.MakeEmojiOutput(1);
            }
        }

        NextEmoji();
    }
}
