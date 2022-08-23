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
    bool grounded = true;
    GameObject resetPoint;
    bool resetting = false;
    Color originalColour;

    void Start()
    {
        //Time.timeScale = 1;
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

        resetPoint = GameObject.Find("ResetPoint");
        originalColour = GetComponent<Renderer>().material.color;
    }

    void FixedUpdate()
    {
        //Checks if the player is resetting their position
        if (resetting)
            return;
        //Checks if player has won the game and disables player movement by returning from the function
        if (wonGame)
            return;
        
        //Checks if the player is still on the ground in order to not allow the build up of speed in mid air
        if (grounded)
        {
            //Store the horizontal axis value in a float
            float moveHorizontal = Input.GetAxis("Horizontal");
            //Store the vertical axis value in a float
            float moveVertical = Input.GetAxis("Vertical");

            //Create a new vector 3 based on the horizontal and vertical values
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            //Add force to our rigidbody from our movement vector times our speed
            rb.AddForce(movement * speed);
        }

        

        
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
            resetPoint.transform.position = other.transform.position;
            

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

    private void OnCollisionStay(Collision collision)
    {
        //When the player stays on a ground collider, Grounded will be set to true
        if (collision.collider.CompareTag("Ground"))
            grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        //When the player leaves a ground collider, Grounded will be set to false
        if (collision.collider.CompareTag("Ground"))
            grounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Respawn"))
        {
            StartCoroutine(ResetPlayer());
        }
    }

    public IEnumerator ResetPlayer()
    {
        resetting = true;
        GetComponent<Renderer>().material.color = Color.black;
        rb.velocity = Vector3.zero;
        Vector3 startPos = transform.position;
        float resetSpeed = 1f;
        var i = 0.0f;
        var rate = 1.0f / resetSpeed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, resetPoint.transform.position, i);
            yield return null;
        }
        GetComponent<Renderer>().material.color = originalColour;
        resetting = false;
        rb.velocity = Vector3.zero;
    }
}
