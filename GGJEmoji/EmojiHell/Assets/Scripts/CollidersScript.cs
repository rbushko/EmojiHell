using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersScript : MonoBehaviour
{
    public float colThickness = 4f;
    public float zPosition = 0f;
    private static Vector2 screenSize;
 
    void Start ()
    {//Create a Dictionary to contain all our Objects/Transforms
        System.Collections.Generic.Dictionary<string,Transform> colliders = new System.Collections.Generic.Dictionary<string,Transform>();
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
            //adding an extra trigger 2D one if it is the bottom
            if (valPair.Key == "Bottom")
            {
                //adding the 1st collider
                //valPair.Value.gameObject.AddComponent<BoxCollider2D>();

                //adding a kinematic rigidbody
                valPair.Value.gameObject.AddComponent<Rigidbody2D>().isKinematic = true;

                //adding the 2nd collider
                BoxCollider2D newCollider = valPair.Value.gameObject.AddComponent<BoxCollider2D>(); //Add our colliders. Remove the "2D", if you would like 3D colliders.
                valPair.Value.name = valPair.Key + "Collider"; //Set the object's name to it's "Key" name, and take on "Collider".  i.e: TopCollider
                valPair.Value.parent = transform; //Make the object a child of whatever object this script is on (preferably the camera)
                newCollider.isTrigger = true;
                valPair.Value.localScale = new Vector3(screenSize.x * 2, colThickness, colThickness);
                valPair.Value.gameObject.AddComponent<ForBottomTriggerReset>();
            }
            else
            {
                valPair.Value.gameObject.AddComponent<BoxCollider2D>(); //Add our colliders. Remove the "2D", if you would like 3D colliders.
                valPair.Value.name = valPair.Key + "Collider"; //Set the object's name to it's "Key" name, and take on "Collider".  i.e: TopCollider
                valPair.Value.parent = transform; //Make the object a child of whatever object this script is on (preferably the camera)
    
                if(valPair.Key == "Left" || valPair.Key == "Right") //Scale the object to the width and height of the screen, using the world-space values calculated earlier
                    valPair.Value.localScale = new Vector3(colThickness, screenSize.y * 2, colThickness);
                else
                    valPair.Value.localScale = new Vector3(screenSize.x * 2, colThickness, colThickness);
            }
            
        }  
    //Change positions to align perfectly with outter-edge of screen, adding the world-space values of the screen we generated earlier, and adding/subtracting them with the current camera position, as well as add/subtracting half out objects size so it's not just half way off-screen
        colliders["Right"].position = new Vector3(cameraPos.x + screenSize.x + (colliders["Right"].localScale.x * 0.5f), cameraPos.y, zPosition);
        colliders["Left"].position = new Vector3(cameraPos.x - screenSize.x - (colliders["Left"].localScale.x * 0.5f), cameraPos.y, zPosition);
        colliders["Top"].position = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (colliders["Top"].localScale.y * 0.5f), zPosition);
        colliders["Bottom"].position = new Vector3(cameraPos.x, cameraPos.y - screenSize.y - (colliders["Bottom"].localScale.y * 0.5f), zPosition);
    
    //setting the tag for the bottom collider
        colliders["Bottom"].gameObject.tag = "Bottom";
        //colliders["Bottom"].gameObject.layer = 12;

    //adding the item generation script to the top here
        colliders["Top"].gameObject.AddComponent<ItemGenerator>();
        colliders["Top"].gameObject.layer = 12;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
