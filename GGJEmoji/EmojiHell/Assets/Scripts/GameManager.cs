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
    public TextMeshProUGUI endReportText;


    //the particle effects and sound effects
    public List<AudioClip> audioClips;
    public AudioSource audioPlayer;
    private AudioSource goodBadAudioPlayer;

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
    private int correctCount;
    private int wrongCount;
    private int wordsTyped;

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
        goodBadAudioPlayer = GetComponent<AudioSource>();
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
        emojis.Clear();
        timeLeft = timePerGame;
        correctCount = 0;
        wrongCount = 0;
        UpdateScore(-score);
        StartCoroutine("Timer");
    }

    public void GameOver()
    {
        endScreen.SetActive(true);
        gameScreen.SetActive(false);
        gameObjects.SetActive(false);
        GenerateEndReport();
        audioPlayer.clip = audioClips[0];
        audioPlayer.Play();
        StopCoroutine("Timer");
    }

    private void GenerateEndReport()
    {
        endReportText.text = "You scored: " + score + "\n" + "Correct: " + correctCount + "\n" + "Incorrect: " + wrongCount;
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
            if (timeLeft == 20)
            {
                audioPlayer.clip = audioClips[1];
                audioPlayer.Play();
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
                goodBadAudioPlayer.clip = audioClips[3];
                goodBadAudioPlayer.Play();
            }
            else
            {
                factory.MakeEmojiOutput(0);
                goodBadAudioPlayer.clip = audioClips[2];
                goodBadAudioPlayer.Play();
            }
            correctCount++;
        }
        else
        {
            //Debug.Log("Nope!");
            UpdateScore(-1);
            if (curEmoji.GetType() == EmojiType.GOOD)
            {
                factory.MakeEmojiOutput(0);
                goodBadAudioPlayer.clip = audioClips[2];
                goodBadAudioPlayer.Play();
            }
            else
            {
                factory.MakeEmojiOutput(1);
                goodBadAudioPlayer.clip = audioClips[3];
                goodBadAudioPlayer.Play();
            }
            wrongCount++;
        }

        NextEmoji();
    }
}
