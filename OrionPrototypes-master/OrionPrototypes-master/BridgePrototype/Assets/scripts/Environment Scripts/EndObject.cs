using UnityEngine;
using System.Collections;

public class EndObject : MonoBehaviour {
	
	// tracks if the player has just entered to be sure we don't immediatly check if they are getting out
	public bool p1justEntered;
    public bool p2justEntered;
	
	// reference to player melding with the rock
	public GameObject player1;
    public GameObject player2;

    //sounds
    public AudioClip unmeld;
    private AudioSource soundSource;

	// reference to the scene's camera
	public GameObject myCamera;
	
	// vector used for movement
	float movementAmount;
	
	// Use this for initialization
	void Start () {
		// setting up the camera from the scene
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera");
        soundSource = GetComponent<AudioSource>();
        movementAmount = 0.0f;
	}

	// Update is called once per frame
    void Update()
    {
        if(player1 && player2)
        {
            movementAmount += 0.1f;
            transform.Rotate(0f,Time.deltaTime * 90f * (movementAmount * 1.1f),0f, Space.Self);
            transform.Translate(Vector3.up * movementAmount/30 * Time.deltaTime);

            if (movementAmount >= 50)
            {
                Application.LoadLevel("EndMenu");
            }
        }
        // checks if the player has just entered, as that way our input check for getting out doesn't immediatly assume the player's input to enter also indicated them exiting
        if (p1justEntered == true)
        {
            p1justEntered = false;
        }
        else
        {
            // if the player pressed square to get out and the rock is on the ground then we put the player on top of the block and turn its renderers and colliders on
            if (player1 && Input.GetButtonDown(player1.GetComponent<PlayerScript>().mySButton) && !player2)
            {
                //play unmeld
                soundSource.PlayOneShot(unmeld, 1.0f);

                player1.transform.position = transform.position + transform.up;
                player1.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player1.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                player1.transform.GetChild(2).GetComponent<Animation>().enabled = true;
                player1.transform.GetComponent<Collider>().enabled = true;

                myCamera.GetComponent<CameraScript>().player1 = player1;
 

                player1 = null;
            }
        }

        //P2
        // checks if the player has just entered, as that way our input check for getting out doesn't immediatly assume the player's input to enter also indicated them exiting
        if (p2justEntered == true)
        {
            p2justEntered = false;
        }
        else
        {
            // if the player pressed square to get out and the rock is on the ground then we put the player on top of the block and turn its renderers and colliders on
            if (player2 && Input.GetButtonDown(player2.GetComponent<PlayerScript>().mySButton) && !player1)
            {
                //play unmeld
                soundSource.PlayOneShot(unmeld, 1.0f);

                player2.transform.position = transform.position + transform.up;
                player2.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player2.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                player2.transform.GetChild(2).GetComponent<Animation>().enabled = true;
                player2.transform.GetComponent<Collider>().enabled = true;

                myCamera.GetComponent<CameraScript>().player2 = player2;

                player2 = null;
            }
        }
    }
}
