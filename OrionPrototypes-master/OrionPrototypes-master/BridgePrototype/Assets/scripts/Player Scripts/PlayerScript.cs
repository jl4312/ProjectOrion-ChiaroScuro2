using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	// a reference to the camera in the scene
	public GameObject myCamera;
	
	// multiplied by the player's movement to alter movement speed
	public float speed;
	
	// multiplied by the upward jump force to alter jump force
	public float jumpheight;
	
	// denotes whether this player is player1 or player 2
	public bool Player1;
	
	// the amount of time that the user has held the jump button (used to allow for users to hold the jump button for bigger jumps)
	private float pressCount;
	
	// the current respawn position in case of player death
	public Vector3 respawnPosition;
	
	// the amount by which the player moves, is set using the gamepad's left analog stick vector
	private Vector3 movementVec;
	
	// denotes whether the player is currently in air
	private bool airborne;
	
	// float used to rotate the movement vector relative to camera orientation
	private float cameraAngleDiff;
	
	// raycast hit used for ground detection
	private RaycastHit hit;
	
	// ray used for ground detection
	private Ray ray;
	
	// the amount of seconds it takes to fade in the meld button prompt
	public float buttonPromptIntroTimer;
	
	// the amount of seconds a button prompt has currently faded in
	private float buttonPromptTimerCurrent;
	
	// the boolean for whether or not the button prompt for meld is showing
	private bool buttonPromptOn;
	
	// used to note the start time of a ui fade in
	private float timeStart;

	// boolean used to check if character is melding or not
	private bool melding;

	private bool meldDone;

	// the amuont of frames into the meld animation
	private int meldingFrames;
	
	// these represent the strings used to leverage Unity's input system and reference inputs in unity's input manager
	public string mySButton;
	public string myXButton;
	public string myTButton;
	public string myOButton;
	public string myShareButton;
	public string myR2Trigger;
	public string myL2Trigger;
	public string myLeftStick;
	public string myRightStick;
	
	// the reference to the spriterenderer for the ui interaction prompt sprite
	private SpriteRenderer interactSprite;

    // a bad habit bool to use
    private bool screwOn;
	
	// the reference to the renderer for the character model
	private Renderer characterRenderer;
	
	// a reference to the character's rigidbody
	private Rigidbody rig;

    //Blane Touched this
    //sounds
    public AudioClip meld;
    public AudioClip walk;
    public AudioClip jump;
    private AudioSource soundSource;

    public Animation anim;

	
	// the initialization of the player
	void Start()
	{
		// setup the needed values for elements of player movement, jumping, and platforming detection
		airborne = false;
		pressCount = 0;
		cameraAngleDiff = 0;
		movementVec = new Vector3(0, 0, 0);
		hit = new RaycastHit();
		
		// set the references so we don't need to continually get components
		interactSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
		rig = GetComponent<Rigidbody>();
		characterRenderer = transform.GetChild(0).GetComponent<Renderer>();

        //Sound References
        soundSource = GetComponent<AudioSource>();

        //Animation
        anim = transform.GetChild(2).GetComponent<Animation>();
        foreach (AnimationState state in anim)
        {
            state.speed = 3.0F;
        }
        anim["Run"].speed = 3f;
        anim.Play();

		// set the ui prompt alpha to 0 so it begins invisible
		Color tempColor = interactSprite.color;
		tempColor.a = 0f;
		interactSprite.color = tempColor;
		
		// gets the camera reference from the scene
		myCamera = GameObject.FindGameObjectWithTag("MainCamera");
		
		// sets up the strings for input so we can properly reference them in the input manager
		if (Player1)
		{
			myOButton = "P1O";
			mySButton = "P1S";
			myTButton = "P1T";
			myXButton = "P1X";
			myR2Trigger = "P1R2";
			myL2Trigger = "P1L2";
			myLeftStick = "P1LeftStick";
			myRightStick = "P1RightStick";
			myShareButton = "P1Share";
		} else
		{
			myOButton = "P2O";
			mySButton = "P2S";
			myTButton = "P2T";
			myXButton = "P2X";
			myR2Trigger = "P2R2";
			myL2Trigger = "P2L2";
			myLeftStick = "P2LeftStick";
			myRightStick = "P2RightStick";
			myShareButton = "P2Share";
		}
		
	}
	
	// the updates for physics-relative checks done on a fixed schedule
	void FixedUpdate()
	{
		
		ray = new Ray(transform.position, -transform.up);
		if (Physics.Raycast(ray, out hit, .7f))
		{
			if (!(hit.collider.isTrigger && hit.collider.gameObject.tag!="Parenting") && hit.transform.gameObject.tag != "Player" )
			{
				airborne = false;

                if (hit.collider.gameObject.tag == "Screw")
                {
                    this.transform.position += new Vector3(0, .01f, 0);
                }
			}
			
		} else
		{
			airborne = true;
		}
	}
	
	// the update loop
	void Update()
	{
		// the child we reference here is our ui prompt sprite, which we always orient towards the camera if it is visible
		if (buttonPromptOn) {
			transform.GetChild (1).forward = (myCamera.transform.forward);
		}
		
		// call the method to display the prompts if nessecary
        DisplayPrompts ();

        transform.up = Vector3.up;
        this.transform.localScale = new Vector3(1,1,1);
        
        if (melding) {
			MeldFrame ();
		} else {
			// start with a quit check to exit the application
			if (Input.GetButton (myShareButton)) {
				Application.Quit ();
			}
		
			// calculate the angle difference between the camera orientation on the xz plane and what is essentially the direction character movement expects to move on
			cameraAngleDiff = Vector3.Angle (new Vector3 (0, 0, 1), new Vector3 (myCamera.transform.forward.x, 0, myCamera.transform.forward.z));
		
			// use cross product to get the sign of the angle at hand
			Vector3 cross = Vector3.Cross (new Vector3 (0, 0, 1), new Vector3 (myCamera.transform.forward.x, 0, myCamera.transform.forward.z));
		
			// if the cross component relative to the xz plane normal (y) is positive or zero then we do nothing, but if it is negative then we negate the angle
			if (cross.y < 0) {
				cameraAngleDiff = -cameraAngleDiff;
			}
		
			// we get the movement vector usign the left analog stick input
			movementVec.x = (Vector3.left * -(Input.GetAxis (myLeftStick + "X") * speed * Time.deltaTime)).x;
			movementVec.z = (Vector3.forward * -(Input.GetAxis (myLeftStick + "Y") * speed * Time.deltaTime)).z;
		
			// we then rotate the movement vector by the camera angle difference we've just calculated
			movementVec = Quaternion.AngleAxis (cameraAngleDiff, Vector3.up) * movementVec;
		
			// translate the player by the finalized movement vector
			this.transform.Translate (movementVec);

			//Play walking sounds
			//Play if not jumping or melding,and moving
			if (new Vector3 (movementVec.x, 0f, movementVec.z) != new Vector3 (0f, 0f, 0f) && !airborne && characterRenderer.enabled == true) {
				if (!soundSource.isPlaying)
					soundSource.Play ();

				if (!anim.IsPlaying ("Run"))
					anim.CrossFade ("Run");
			} else {
				//dont stop other sounds
				if (!airborne && characterRenderer.enabled == true) {
					soundSource.Stop ();
					anim.CrossFade ("Idle");
				}
			}
			//soundSource.loop = true;
		
			// if the player is trying to jump and we aren't already airborne
			if (Input.GetButtonDown (myXButton) && airborne == false) {
				// also check to make sure that we haven't passed the upper limit for how long players can hold the jump button, then the jump occurs
				if (pressCount <= 5f) {
					soundSource.Stop ();
					soundSource.PlayOneShot (jump, 1.0f);
					anim.CrossFade ("Jump");
					Jump ();
					pressCount++;
				}
			}
		
			// the moment players let go of the jump button we reset the counter for how long they've held
			if (Input.GetButtonUp (myXButton)) {
				anim.CrossFade ("Land");
				pressCount = 0;
			}
		
			// instead of rotating the entire player we just rotate the model
			if (Input.GetAxis (myLeftStick + "X") > .1f || Input.GetAxis (myLeftStick + "Y") > .1f || Input.GetAxis (myLeftStick + "X") < -.1f || Input.GetAxis (myLeftStick + "Y") < -.1f) {
				this.gameObject.transform.GetChild (2).forward = movementVec;
			}
		}
	}
	
	// the display prompts function goes through the process of displaying the ui prompt properly and fading it in if need be
	void DisplayPrompts()
	{
		// if the prompt is on and we haven't finished fading it, we keep increasing it
		if (buttonPromptOn && buttonPromptTimerCurrent < buttonPromptIntroTimer)
		{
			buttonPromptTimerCurrent += (Time.time - timeStart);
			
			Color tempColor = interactSprite.color;
			tempColor.a = (buttonPromptTimerCurrent / buttonPromptIntroTimer);
			interactSprite.color = tempColor;
		} 
		// if we aren't displaying the prompt then the alpha is just set back to zero
		else
		{
			Color tempColor = interactSprite.color;
			tempColor.a = 0f;
			interactSprite.color = tempColor;
			buttonPromptTimerCurrent = 0;
		}
	}
	
	// the jump function simply gives the velocity of the character a great increase (mimicing the acceleration being applied almost all instantly) and we set airborn to true
	void Jump()
	{
		rig.velocity += jumpheight * transform.up;
		airborne = true;
	}
	
	// the respawn function just sets the position back to the last respawn point
	public void Respawn()
	{
		transform.position = respawnPosition;
	}
	
	// collision checks, which only matter for the balls fired at players marked "Harmful"
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Harmful")
		{
			// we have to first check the color of the ball, as if it matches the color of the player then it is destroyed with no consequence, otherwise it kills the player
			ColorScript colorComponent = col.gameObject.GetComponent<ColorScript>();
			if ((colorComponent && colorComponent.IsMyColor(Player1)))
			{
				col.gameObject.GetComponent<BallScript>().DestroyBall();
			} else if (!colorComponent || (colorComponent && !colorComponent.IsMyColor(Player1)))
			{
				Respawn();
			}
		}
	}
	
	// trigger checks, which all have to do with potential melding objects
	void OnTriggerStay(Collider col)
	{
		ColorScript colorComponent = col.gameObject.GetComponent<ColorScript>();
		
		// firstly if the object has a color attribute and it doesn't match the players then it can't be used
		if (!colorComponent || (colorComponent && ((colorComponent.isWhite && !Player1) || (!colorComponent.isWhite && Player1))))
		{
			// here we display the ui prompt for entering a meld-able object now that we're close enough
            if ((col.gameObject.tag == "Screw" && !col.GetComponentInParent<ScrewScript>().player) || (col.gameObject.tag == "SingleControlRock" && !col.GetComponent<SingleControlRock>().player1) || (col.gameObject.tag == "Switch" && !col.GetComponentInParent<ScrewScript>().player) || (col.gameObject.tag == "EndObject" && (!col.GetComponentInParent<EndObject>().player1 || !col.GetComponentInParent<EndObject>().player2)))
			{
				buttonPromptOn = true;
				timeStart = Time.time;
			} else {
				buttonPromptOn = false;
			}

            // if the player hits the square button and the object nearby is meld-able (i.e. a switch, screw, or control rock), then we call the meld in function to transition the player there
            if (col.gameObject.tag == "Switch" && (Input.GetButtonDown(mySButton)))
			{
				if (!col.GetComponentInParent<SwitchScript>().player1)
				{
					anim.CrossFade("Meld");
					melding = true;
				}
			}
			
			if (col.gameObject.tag == "Screw")
			{
				// if they are colliding and pressing the enter button we begin the meld process
				if ((Input.GetButtonDown(mySButton)) && !col.GetComponentInParent<ScrewScript>().player && !col.GetComponentInParent<ScrewScript>().locked && !melding && !meldDone)
				{
					this.gameObject.transform.GetChild (2).forward = (col.transform.position - transform.position);
					anim.CrossFade("Meld");
					melding = true;
					buttonPromptOn = false;
                    col.GetComponentInParent<ScrewScript>().locked = true;
                }
				// otherwise, if they are in an object range and have gone through a meld animation, we transfer the player to the object
				else if (meldDone){
					col.GetComponentInParent<ScrewScript>().player = this.gameObject;
					col.GetComponentInParent<ScrewScript>().justEntered = true;
					meldDone = false;

					//Play meld sound
					soundSource.Stop();
					soundSource.PlayOneShot(meld, 1.0f);
					
					//Legacy animations dont support events on animation, so there is no way
					//to make the animation play before this happens. 
					//Making this function an IEnumerator will break it as well
					
					characterRenderer.enabled = false;
					transform.GetComponent<Collider>().enabled = false;
					if (Player1)
					{
						myCamera.GetComponent<CameraScript>().player1 = col.gameObject;
                    } else
                    {
                        myCamera.GetComponent<CameraScript>().player2 = col.gameObject;
                    }

                    col.GetComponentInParent<ScrewScript>().locked = false;

                    buttonPromptOn = false;

					this.transform.position -= new Vector3(0,100,0);
				}
			}
			
			if (col.gameObject.tag == "SingleControlRock")
			{
				// if they are colliding and pressing the enter button we begin the meld process
				if (Input.GetButtonDown(mySButton) && !col.GetComponent<SingleControlRock>().player1 && !col.GetComponent<SingleControlRock>().locked && !melding && !meldDone)
				{
					this.gameObject.transform.GetChild (2).forward = (col.transform.position - transform.position);
					anim.CrossFade("Meld");
					melding = true;
                    col.GetComponent<SingleControlRock>().locked = true;
                    buttonPromptOn = false;
				}
				// otherwise, if they are in an object range and have gone through a meld animation, we transfer the player to the object
				else if (meldDone){
					col.GetComponentInParent<SingleControlRock>().player1 = this.gameObject;
					col.GetComponentInParent<SingleControlRock>().justEntered = true;

					//Play meld sound
					soundSource.Stop();
					soundSource.PlayOneShot(meld, 1.0f);
					
					//Legacy animations dont support events on animation, so there is no way
					//to make the animation play before this happens. 
					//Making this function an IEnumerator will break it as well
					
					characterRenderer.enabled = false;
					transform.GetComponent<Collider>().enabled = false;
					if (Player1)
					{
						myCamera.GetComponent<CameraScript>().player1 = col.gameObject;
                    } else
                    {
                        myCamera.GetComponent<CameraScript>().player2 = col.gameObject;
                    }

                    col.GetComponent<SingleControlRock>().locked = false;

                    this.transform.position -= new Vector3(0,100,0);

					meldDone = false;
                }
			}

            if (col.gameObject.tag == "EndObject")
            {
				// if they are colliding and pressing the enter button we begin the meld process
				if ((Input.GetButtonDown(mySButton)))
                {
					this.gameObject.transform.GetChild (2).forward = new Vector3((col.transform.position - transform.position).x,0,(col.transform.position - transform.position).z);
					anim.CrossFade("Meld");
					melding = true;
					buttonPromptOn = false;
                }
				// otherwise, if they are in an object range and have gone through a meld animation, we transfer the player to the object
                else if (meldDone)
                {					
					//Play meld sound
					soundSource.Stop();
					soundSource.PlayOneShot(meld, 1.0f);
					
					//Legacy animations dont support events on animation, so there is no way
					//to make the animation play before this happens. 
					//Making this function an IEnumerator will break it as well
					
					characterRenderer.enabled = false;
					transform.GetComponent<Collider>().enabled = false;
					if (Player1)
					{
						myCamera.GetComponent<CameraScript>().player1 = col.gameObject;
					} else
					{
						myCamera.GetComponent<CameraScript>().player2 = col.gameObject;
                    }
                    
                    this.transform.position -= new Vector3(0,100,0);
                    
                    meldDone = false;
                    if(Player1){
						col.GetComponentInParent<EndObject>().player1 = this.gameObject;
                    }
                    else 
                    {
						col.GetComponentInParent<EndObject>().player2 = this.gameObject;
                    }
                }
            }
			
			if (transform.GetComponent<Collider>().enabled == false)
			{
				buttonPromptOn = false;
			}
		}
	}

	void MeldFrame(){
		meldingFrames++;
		
		if (meldingFrames > 75) {
			melding = false;
			meldingFrames = 0;
			meldDone = true;
		}
	}
	
	// if the player exits the trigger of meld-able objects then we get rid of the button prompt to enter
	void OnTriggerExit(Collider col)
	{
        if (col.gameObject.tag == "Screw" || col.gameObject.tag == "SingleControlRock" || col.gameObject.tag == "Switch" || col.gameObject.tag == "EndObject")
		{
			buttonPromptOn = false;
		}
	}
}
