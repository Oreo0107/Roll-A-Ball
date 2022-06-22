using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 1.0f;
    private int pickupCount;
    int totalPickups;
    private bool wonGame = false;
    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text winText;
    public GameObject inGamePanel;
    public GameObject winPanel;
    public Image pickupFill;
    float pickupChunk;

    void Start()
    {
        //Turn off our winPanel object
        winPanel.SetActive(false);
        //turn on our ingamepanel
        inGamePanel.SetActive(true);
        //Gets the rigid body component attached to this game object
        rb = GetComponent<Rigidbody>();
        //Work out how many pickups are in the scene and store in variable (pickupCount)
        pickupCount = GameObject.FindGameObjectsWithTag("Pickup").Length;
        //Assign the amount of pickups to the total pickups
        totalPickups = pickupCount;
        //Work out the amount of fill for our pickupFill
        pickupChunk = 1.0f / pickupCount;
        pickupFill.fillAmount = 0;
        //Display the pickups to the user
        CheckPickups();
    }

    void FixedUpdate()
    {
        //Checks if player has won the game and disables player movement by returning from the function
        if (wonGame)
            return;

        //Store the horizontal axis value in a float
        float moveHorizontal = Input.GetAxis("Horizontal");
        //Store the vertical axis value in a float
        float moveVertical = Input.GetAxis("Vertical");

        //Create a new vector 3 based on the horizontal and vertical values
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //Add force to our rigidbody from our movement vector times our speed
        rb.AddForce(movement * speed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            //Decrement the pickupCount when we collide with a pickup
            pickupCount -= 1;
            //Display the pickups to the user
            //Increase the fill amount of our pickupFill image
            pickupFill.fillAmount = pickupFill.fillAmount + pickupChunk;
            CheckPickups();

            Destroy(other.gameObject);
        }
    }

    void CheckPickups()
    {
        //Display the new pickupCount to the player
        scoreText.text = "Items Left: " + pickupCount.ToString() + "/" + totalPickups.ToString();
        //Check if the pickupCount == 0
        if (pickupCount == 0)
        {
            //If pickupCount == 0, display win message remove controls from player
            winPanel.SetActive(true);
            //Turn off our inGamePanel
            inGamePanel.SetActive(false);
            //Remove controls from player
            wonGame = true;
            //Set the velocity of the rigidbody to 0
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }



    //Creat a win condition that happens when pickupCount == 0
    //temproray reset functionality
    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

}
