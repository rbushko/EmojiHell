using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmojiFactory : MonoBehaviour
{
    public GameObject cloneMoji;
    public List<Transform> outputTransforms; 
    public List<GameObject> particleEffects;

    private System.Random rand;

    public float colThickness = 4f;
    public float zPosition = 0f;
    private static Vector2 screenSize;
    private Dictionary<string,Transform> colliders;

    private void Start() 
    {

        rand = new System.Random();
        //Create a Dictionary to contain all our Objects/Transforms
        colliders = new Dictionary<string,Transform>();
        //Create our GameObjects and add their Transform components to the Dictionary we created above
        colliders.Add("Top",new GameObject().transform);
        colliders.Add("Bottom",new GameObject().transform);
        colliders.Add("Right",new GameObject().transform);
        colliders.Add("Left",new GameObject().transform);
        //Generate world space point information for position and scale calculations
        Vector3 cameraPos = Camera.main.transform.position;
        //Grab the world-space position values of the start and end positions of the screen, 
        //then calculate the distance between them and store it as half, since we only need half that value for distance away from the camera to the edge
        screenSize.x = Vector2.Distance (Camera.main.ScreenToWorldPoint(new Vector2(0,0)),Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f; 
        screenSize.y = Vector2.Distance (Camera.main.ScreenToWorldPoint(new Vector2(0,0)),Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
        //For each Transform/Object in our Dictionary
        foreach(KeyValuePair<string,Transform> valPair in colliders)
        {
            BoxCollider2D newCollider = valPair.Value.gameObject.AddComponent<BoxCollider2D>(); //Add our colliders. Remove the "2D", if you would like 3D colliders.
            valPair.Value.name = valPair.Key + "Collider"; //Set the object's name to it's "Key" name, and take on "Collider".  i.e: TopCollider
            valPair.Value.parent = transform; //Make the object a child of whatever object this script is on (preferably the camera)
            newCollider.isTrigger = true;
            if(valPair.Key == "Left" || valPair.Key == "Right") //Scale the object to the width and height of the screen, using the world-space values calculated earlier
            {
                valPair.Value.localScale = new Vector3(colThickness, screenSize.y * 3, colThickness);
                valPair.Value.gameObject.tag = "despawn";
            }
            else
            {
                valPair.Value.localScale = new Vector3(screenSize.x * 3, colThickness, colThickness);
            }
        }
        //Change positions to align perfectly with outter-edge of screen, adding the world-space values of the screen we generated earlier, and adding/subtracting them with the current camera position, as well as add/subtracting half out objects size so it's not just half way off-screen
        colliders["Right"].position = new Vector3(cameraPos.x + screenSize.x + (colliders["Right"].localScale.x * 0.5f), cameraPos.y, zPosition);
        colliders["Left"].position = new Vector3(cameraPos.x - screenSize.x - (colliders["Left"].localScale.x * 0.5f), cameraPos.y, zPosition);
        colliders["Top"].position = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (colliders["Top"].localScale.y * 0.5f), zPosition);
        colliders["Bottom"].position = new Vector3(cameraPos.x, cameraPos.y - screenSize.y - (colliders["Bottom"].localScale.y * 0.5f), zPosition);
        colliders["Bottom"].gameObject.tag = "despawn";
    }

    public void MakeEmojiInput()
    {
        EmojiType type = (EmojiType) rand.Next(2);
        List<string> choices = new List<string>();
        int indx = GetChoiceList(type, choices);
        //List<string> choices = null;
        GameObject newEmoji = Instantiate(cloneMoji, colliders["Top"].position, Quaternion.identity);
        newEmoji.GetComponent<EmojiHelper>().Init(type, choices, new Vector3(rand.Next(-1,1), 0, 0), indx);
    }

    //takes in true for if the emoji was given a good fate and false if given a bad fate
    public void MakeEmojiOutput(int isGood)
    {
        EmojiType type = (EmojiType) (EmojiType.BADOUT + isGood);
        GameObject newEmoji = Instantiate(cloneMoji, outputTransforms[isGood].position, Quaternion.identity);
        Vector3 temp = new Vector3(outputTransforms[isGood].position.x, outputTransforms[isGood].position.y - 1f + (2*isGood), 0);
        Instantiate(particleEffects[isGood], temp, Quaternion.identity);
        Vector3 vel = new Vector3(0,0,0);
        if (isGood == 0)
        {
            vel = new Vector3(2, 0, 0);
        }
        else
        {
            vel = new Vector3(-2, 25, 0);
        }
        //newEmoji.transform = outputTransforms[isGood];
        newEmoji.GetComponent<EmojiHelper>().Init(type, null, vel, -1);
    }

    private int GetChoiceList(EmojiType type, List<string> choices)
    {
        List<string> otherOptions;

        string toAdd = "";
        
        if (type == EmojiType.GOOD)
        {
            toAdd = AssetManager.GetRandomGoodItem();
            otherOptions = AssetManager.GetBadList();
        }
        else
        {
            toAdd = AssetManager.GetRandomBadItem();
            otherOptions = AssetManager.GetGoodList();
        }

        // Create a list of 4 choices with unique first letters
        while (choices.Count < 3)
        {
            bool addPossible = true;
            string possibleOption = otherOptions[rand.Next(otherOptions.Count)];

            if (addPossible)
            {
                choices.Add(possibleOption);
            }
        }

        int correctIndx = rand.Next(3);
        choices.Insert(correctIndx, toAdd);

        return correctIndx;
    }
}
