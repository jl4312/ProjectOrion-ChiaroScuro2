using UnityEngine;
using System.Collections;

public class SingleControlRock : MonoBehaviour {
	
	// tracks if the player has just entered to be sure we don't immediatly check if they are getting out
	public bool justEntered;
	
	// reference to player melding with the rock
	public GameObject player1;
	
	// rock's jump height value
	public float jumpHeight;

    // the boolean to lock the rock from access for the other player
    public bool locked;

	public bool timing;

	public float timer;

	public float tStart;

	public float timerFull;

    //sounds
    public AudioClip jump;
    public AudioClip unmeld;
    private AudioSource soundSource;
	
	// applies to movement vector to adjust how fast the rock can move at a time
	private float speed;
	
	// the ray and hit used to check the ground below the rock for platforming purposes
	private Ray ray;
	private RaycastHit hit;
	
	// reference to the scene's camera
	public GameObject myCamera;
	
	// angle between the camera projected ono the xz plane and the z cardinal vector projected onto the plane, used for movement with camera perspective
	private float cameraAngleDiff;

	private float max;
	
	// vector used for movement of the rock
	Vector3 movementVec;
	
	// Use this for initialization
	void Start () {
		// setting up the camera from the scene
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		speed = 2;
        soundSource = GetComponent<AudioSource>();
		timerFull = 3;
	}
	
	void FixedUpdate(){
		// apply gravitational force to the rock
		GetComponent<Rigidbody>().AddForce(0,-6.5f,0);

		GetComponent<Rigidbody> ().velocity = new Vector3 (GetComponent<Rigidbody> ().velocity.x, Mathf.Min(GetComponent<Rigidbody> ().velocity.y, 6.0f), GetComponent<Rigidbody> ().velocity.z);
	}
	
	// Update is called once per frame
	void Update () {

		if(locked && !timing){
			timing = true;
			tStart = Time.time;
		}

		if(timing){
			timer = Time.time - tStart;
			if(timer>timerFull){
				locked = false;
				timing = false;
			}
		}

		// checks if the player has just entered, as that way our input check for getting out doesn't immediatly assume the player's input to enter also indicated them exiting
		if (justEntered == true) {
			justEntered = false;
		} else {
			// if the player pressed square to get out and the rock is on the ground then we put the player on top of the block and turn its renderers and colliders on
			if (player1 && Input.GetButtonDown (player1.GetComponent<PlayerScript> ().mySButton)) {
                //play unmeld
                soundSource.PlayOneShot(unmeld, 1.0f);

				player1.transform.position = transform.position + transform.up;
				player1.transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				player1.transform.GetChild (0).GetComponent<Renderer> ().enabled = true;
                player1.transform.GetChild(2).GetComponent<Animation>().enabled = true;
				player1.transform.GetComponent<Collider> ().enabled = true;
				
				if(myCamera.GetComponent<CameraScript> ().player1 == this.gameObject){
					myCamera.GetComponent<CameraScript> ().player1 = player1;
				}else {
					myCamera.GetComponent<CameraScript> ().player2 = player1;
				}
				
				player1 =null;
			}
		}
		
		// if the player is inhabiting the rock, we use a control system identical to the player to control movement, see player class for more specifics
		if (player1) {
			cameraAngleDiff = Vector3.Angle (new Vector3 (0, 0, 1), new Vector3 (myCamera.transform.forward.x, 0, myCamera.transform.forward.z));
			
			Vector3 cross = Vector3.Cross (new Vector3 (0, 0, 1), new Vector3 (myCamera.transform.forward.x, 0, myCamera.transform.forward.z));
			
			if (cross.y < 0) {
				cameraAngleDiff = -cameraAngleDiff;
			}
			movementVec.x = (Vector3.left * -(Input.GetAxis (player1.GetComponent<PlayerScript>().myLeftStick + "X")* Time.deltaTime)).x;
			movementVec.z = (Vector3.forward * -(Input.GetAxis (player1.GetComponent<PlayerScript>().myLeftStick + "Y")* Time.deltaTime)).z;
			
			movementVec = Quaternion.AngleAxis(cameraAngleDiff, Vector3.up) * movementVec;
			
			movementVec *= speed;

			if(movementVec.magnitude>0){
				this.transform.Translate(movementVec);
			}
		}
		
		// raycast check used to see if block is on the gruond
		if(Physics.Raycast(ray, out hit, .7f)){
			if(hit.collider.gameObject.tag != "Player" && player1 && Input.GetButton(player1.GetComponent<PlayerScript>().myXButton)){
				if(!(hit.collider.isTrigger && hit.collider.gameObject.tag!="Parenting")){
                    //play jump
                    if(!soundSource.isPlaying)
                        soundSource.PlayOneShot(jump, 1.0f);

					GetComponent<Rigidbody>().velocity += jumpHeight * transform.up;
				}
			}
		}
		
		ray = new Ray(transform.position + transform.GetChild(0).GetComponent<Collider>().bounds.extents, -transform.up);

        // raycast check used to see if block is on the gruond
        if (Physics.Raycast(ray, out hit, .7f))
        {
            if (hit.collider.gameObject.tag != "Player" && player1 && Input.GetButton(player1.GetComponent<PlayerScript>().myXButton))
            {
                if (!(hit.collider.isTrigger && hit.collider.gameObject.tag != "Parenting"))
                {
                    //play jump
                    if (!soundSource.isPlaying)
                        soundSource.PlayOneShot(jump, 1.0f);

                    GetComponent<Rigidbody>().velocity += jumpHeight * transform.up;
                }
            }
        }

        ray = new Ray(transform.position - transform.GetChild(0).GetComponent<Collider>().bounds.extents, -transform.up);

        // raycast check used to see if block is on the gruond
        if (Physics.Raycast(ray, out hit, .7f))
        {
            if (hit.collider.gameObject.tag != "Player" && player1 && Input.GetButton(player1.GetComponent<PlayerScript>().myXButton))
            {
                if (!(hit.collider.isTrigger && hit.collider.gameObject.tag != "Parenting"))
                {
                    //play jump
                    if (!soundSource.isPlaying)
                        soundSource.PlayOneShot(jump, 1.0f);

                    GetComponent<Rigidbody>().velocity += jumpHeight * transform.up;
                }
            }
        }

        ray = new Ray(transform.position + new Vector3(-transform.GetChild(0).GetComponent<Collider>().bounds.extents.x, 0 , transform.GetChild(0).GetComponent<Collider>().bounds.extents.z), -transform.up);

        // raycast check used to see if block is on the gruond
        if (Physics.Raycast(ray, out hit, .7f))
        {
            if (hit.collider.gameObject.tag != "Player" && player1 && Input.GetButton(player1.GetComponent<PlayerScript>().myXButton))
            {
                if (!(hit.collider.isTrigger && hit.collider.gameObject.tag != "Parenting"))
                {
                    //play jump
                    if (!soundSource.isPlaying)
                        soundSource.PlayOneShot(jump, 1.0f);

                    GetComponent<Rigidbody>().velocity += jumpHeight * transform.up;
                }
            }
        }

        ray = new Ray(transform.position - new Vector3(-transform.GetChild(0).GetComponent<Collider>().bounds.extents.x, 0, transform.GetChild(0).GetComponent<Collider>().bounds.extents.z), -transform.up);

        // raycast check used to see if block is on the gruond
        if (Physics.Raycast(ray, out hit, .7f))
        {
            if (hit.collider.gameObject.tag != "Player" && player1 && Input.GetButton(player1.GetComponent<PlayerScript>().myXButton))
            {
                if (!(hit.collider.isTrigger && hit.collider.gameObject.tag != "Parenting"))
                {
                    //play jump
                    if (!soundSource.isPlaying)
                        soundSource.PlayOneShot(jump, 1.0f);

                    GetComponent<Rigidbody>().velocity += jumpHeight * transform.up;
                }
            }
        }

        ray = new Ray(transform.position, -transform.up);
    }
}
