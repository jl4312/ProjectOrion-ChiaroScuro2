using UnityEngine;
using System.Collections;

public class ScrewScript : MonoBehaviour {
	
	// boolean used for tracking when we hit the first update when a player enters
	public bool justEntered;
	
	// reference to the player inhabiting the screw
	public GameObject player;
	
	// reference to the camera
	public GameObject myCamera;
	
	// reference to the top limit area of the screw
	public GameObject topLimit;
	
	// the float that influences how quickly the screw moves up
	public float risingSpeed;

    // the boolean to lock the screw from access for the other player
    public bool locked;

    // the float that influences how quicky the screw moves down
    public float descendingSpeed;
	
	// the float that denotes how many seconds the screw holds in place before descending
	public float hoverTime;
	
	// denotes whether the player can screw down
	public bool canGoDown;

    //Sounds
    public AudioClip screwSound;
    public AudioClip unmeld;
    private AudioSource soundSource;
	
	// tracks analog stick positioning
	private Vector2 analogStickPos;
	
	// tracks previous stick position to get change in stick movement
	private Vector2 prevAnalogStickPos;
	
	// the float that tracks the difference in stick movement
	private float angleDiff;
	
	// the location that denotes the minimal spot the screw can go down to.
	private Vector3 bottomY;
	
	// tracks how long the screw has been hovering
	private float currentHoverValue;
	
	// tracks the overall time it takes for the UI prompt to fade in
	public float rotatePromptIntroTimer;
	
	// tracks the current amuont of time that the UI has been fading in
	private float rotatePromptTimerCurrent;
	
	// tracks if the UI prompt is active
	private bool rotatePromptOn;
	
	// tracks the start time of the UI prompt fade in
	private float timeStart;
	
	// denotes whether the collider rotates with the screw to ease walking
	public bool ColliderRotates;
	
	// reference to the UI rotation prompt
	private GameObject rotationSprite;
	
	// tracks if the screw is only meant to rotate in place
	public bool spinsInPlace;
	
	// tracks the current amount that the screw is spun
	public float spinVal;
	
	// We set up our initial camera reference, instantiate the values tracking input and position, and also set up the UI prompt as transparent
	void Start () {
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		prevAnalogStickPos = new Vector2 (0, 0);
		bottomY = transform.GetChild(0).position;
		currentHoverValue = hoverTime;
		Color tempColor = transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer> ().color;
		tempColor.a = 0f;
		transform.GetChild (0).GetChild (3).GetComponent<SpriteRenderer> ().color = tempColor;
		tempColor = transform.GetChild(0).GetChild(4).GetComponent<SpriteRenderer> ().color;
		tempColor.a = 0f;
		transform.GetChild (0).GetChild (4).GetComponent<SpriteRenderer> ().color = tempColor;
		spinVal = 0;
        soundSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		// doing a just entered check to make it so we do initial setup.
		if (justEntered == true) {
			justEntered = false;
			rotatePromptOn = true;
			rotatePromptTimerCurrent =0;
			timeStart = Time.time;
		} else {
			// setting the display prompts to show correctly to the camera.
		//	transform.GetChild(0).GetChild(3).forward = (myCamera.transform.forward);
		//	transform.GetChild(0).GetChild(4).forward = (myCamera.transform.forward);
			
			DisplayPrompts();
			
			// resetting it so that it is never below the bottom area
			if(Vector3.Dot(transform.GetChild(0).position, this.transform.up)<Vector3.Dot(bottomY, this.transform.up)){
				transform.GetChild(0).position=new Vector3(bottomY.x,bottomY.y,bottomY.z);
			}
			
			// reset the angle diff value for each update so we can isolate the change for just that update
			angleDiff = 0;
			
			// handle players leaving the screw, which involves placing them nearby and re-enabling all their components
			if (player && Input.GetButtonDown (player.GetComponent<PlayerScript> ().mySButton)) {
                //play unmeld
                soundSource.PlayOneShot(unmeld, 1.0f);

				player.transform.position = transform.GetChild(3).position;
				player.transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				player.transform.GetComponent<Rigidbody> ().useGravity = true;
				player.transform.GetChild (0).GetComponent<Renderer> ().enabled = true;
                player.transform.GetChild(2).GetComponent<Animation>().enabled = true;
				player.transform.GetComponent<Collider> ().enabled = true;
				
				if(myCamera.GetComponent<CameraScript> ().player1 == this.transform.GetChild(2).gameObject){
					myCamera.GetComponent<CameraScript> ().player1 = player;
				}else {
					myCamera.GetComponent<CameraScript> ().player2 = player;
				}
				
				rotatePromptOn = false;
				rotatePromptTimerCurrent =0;
				
				player = null;
			}
			
			//If there's a player then we check for input for rotation to see if they are moving the screw
			if(player){
				analogStickPos = new Vector2(Input.GetAxis(player.GetComponent<PlayerScript>().myLeftStick + "X"),Input.GetAxis(player.GetComponent<PlayerScript>().myLeftStick + "Y"));
				
				// if the analog stick is on its fringes (over 95% extended) then we will check if it is being spun
				if(Mathf.Abs(prevAnalogStickPos.magnitude)>.95f && Mathf.Abs(analogStickPos.magnitude)>.95f){
					// we use a cross here between the resulting vector tels us if it is being rotated up or down
					if(Vector3.Cross(new Vector3(prevAnalogStickPos.x,prevAnalogStickPos.y),new Vector3(analogStickPos.x,analogStickPos.y)).z<0){
						// we get rid of the prompt once players start rotating the screw
						if(rotatePromptOn) {
							rotatePromptOn = false;
							rotatePromptTimerCurrent =0;
						}
						
						// we reset the hover time every time players move the screw
						currentHoverValue = hoverTime;
						angleDiff = Vector2.Angle(prevAnalogStickPos,analogStickPos);
					}
					// if the players can go down and aren't lower than the bottom limit then we decrement the angleDiff
					else if(canGoDown && Vector3.Dot(transform.GetChild(0).position, this.transform.up)>Vector3.Dot(bottomY, this.transform.up)) {
						currentHoverValue = hoverTime;
						angleDiff = -Vector2.Angle(prevAnalogStickPos,analogStickPos);
					}
				}
			}
			
			// making the previous position vector equal to the current for next check
			prevAnalogStickPos = analogStickPos;
			
			// this chunk controls the actual rotation and movement of the screw from the data collected about angleDiff directly above
			if(Vector3.Dot(transform.GetChild(0).position, this.transform.up)<Vector3.Dot(topLimit.transform.position, this.transform.up) && Vector3.Dot(transform.GetChild(0).position, this.transform.up)>=Vector3.Dot(bottomY, this.transform.up)){
               //Play screw Sound
                if(angleDiff != 0.0f)
                {
                    if (!soundSource.isPlaying)
                         soundSource.Play();
                }
				// we need only care about rotation if spinsInPlace is true
				if(spinsInPlace){
					transform.GetChild(0).RotateAround(transform.GetChild(0).position,(transform.GetChild(1).position - transform.GetChild(0).position),-(angleDiff * risingSpeed * .5f));
					spinVal += -(angleDiff * risingSpeed * .5f);
				} 
				else {
					// if the collider rotates then we rotate the entire child Mesh group and change position
					if(ColliderRotates){
						transform.GetChild(0).RotateAround(transform.GetChild(0).position,(transform.GetChild(1).position - transform.GetChild(0).position),-(angleDiff * risingSpeed * .5f));
						transform.GetChild(0).position += (new Vector3(0,(angleDiff* risingSpeed *.0005f),0).magnitude * this.transform.up);
					}
					// otherwise we only pick out the renderer transformes and rotate them and change position
					else{
						Renderer[] rS = transform.GetChild(0).GetComponentsInChildren<Renderer>();
						for(int i = 0;i<rS.Length;i++){
							rS[i].transform.RotateAround(transform.GetChild(0).position,(transform.GetChild(1).position - transform.GetChild(0).position),-(angleDiff * risingSpeed * .5f));
						}
						if(angleDiff>0){
							transform.GetChild(0).position += (new Vector3(0,(angleDiff* risingSpeed *.0005f),0).magnitude * this.transform.up);
						} else {
							transform.GetChild(0).position -= (new Vector3(0,(angleDiff* risingSpeed *.0005f),0).magnitude * this.transform.up);
						}
					}
				}
			}
			
			// this chunk covers the screw hovering and descending after its hover period
			if(Vector3.Dot(transform.GetChild(0).position, this.transform.up)>Vector3.Dot(bottomY, this.transform.up) && angleDiff==0){
				// going through the hover period
				if(currentHoverValue>0){
					currentHoverValue -=.1f;
				} 
				// if we are no longer hovering, then the screw is rotated and move downward
				else
				{
                    
					if(ColliderRotates){
						transform.GetChild(0).RotateAround(transform.GetChild(0).position,(transform.GetChild(1).position - transform.GetChild(0).position),descendingSpeed);
						transform.GetChild(0).position -= (new Vector3(0,(.001f * descendingSpeed),0).magnitude * this.transform.up);
					} else {
						Renderer[] rS = transform.GetChild(0).GetComponentsInChildren<Renderer>();
						for(int i = 0;i<rS.Length;i++){
							rS[i].transform.RotateAround(transform.GetChild(0).position,(transform.GetChild(1).position - transform.GetChild(0).position),descendingSpeed);
						}
						//transform.GetChild(0).GetComponentInChildren<Renderer>().transform.RotateAround(transform.GetChild(0).position,-(transform.GetChild(1).position - transform.GetChild(0).position),descendingSpeed);
						transform.GetChild(0).position -= (new Vector3(0,(.001f * descendingSpeed),0).magnitude * this.transform.up);
					}
				}
			} 
			// if we're already at the bottom then we consider the possibility of spinsInPlace being true
			else {
				if(currentHoverValue>0){
					currentHoverValue -=.1f;
				} else {
					if(spinsInPlace && spinVal<0){
						transform.GetChild(0).RotateAround(transform.GetChild(0).position,(transform.GetChild(1).position - transform.GetChild(0).position),descendingSpeed);
						spinVal += descendingSpeed;
					}
				}
			}
		}
	}
	
	
	//Show the UI "above" the screw
	void DisplayPrompts(){
		if (player) {
			// if the prompt is on then we fade it in until the timer is hit, and if it is off then we fade it out until the timer is hit
			if (rotatePromptOn) {
				if (rotatePromptTimerCurrent < rotatePromptIntroTimer) {
					rotatePromptTimerCurrent += (Time.time - timeStart);
					
					if (player.GetComponent<PlayerScript> ().Player1) {
						Color tempColor = transform.GetChild (0).GetChild (3).GetComponent<SpriteRenderer> ().color;
						tempColor.a = (rotatePromptTimerCurrent / rotatePromptIntroTimer);
						transform.GetChild (0).GetChild (3).GetComponent<SpriteRenderer> ().color = tempColor;
					} else {
						Color tempColor = transform.GetChild (0).GetChild (4).GetComponent<SpriteRenderer> ().color;
						tempColor.a = (rotatePromptTimerCurrent / rotatePromptIntroTimer);
						transform.GetChild (0).GetChild (4).GetComponent<SpriteRenderer> ().color = tempColor;
					}
				}
			} else {
				if (rotatePromptTimerCurrent < rotatePromptIntroTimer) {
					rotatePromptTimerCurrent += (Time.time - timeStart);
					
					if (player.GetComponent<PlayerScript> ().Player1) {
						Color tempColor = transform.GetChild (0).GetChild (3).GetComponent<SpriteRenderer> ().color;
						tempColor.a = 1 - (rotatePromptTimerCurrent / rotatePromptIntroTimer);
						transform.GetChild (0).GetChild (3).GetComponent<SpriteRenderer> ().color = tempColor;
					} else {
						Color tempColor = transform.GetChild (0).GetChild (4).GetComponent<SpriteRenderer> ().color;
						tempColor.a = 1 - (rotatePromptTimerCurrent / rotatePromptIntroTimer);
						transform.GetChild (0).GetChild (4).GetComponent<SpriteRenderer> ().color = tempColor;
					}
				}
			}
		}
		// if there's no player, there should be no prompt showing
		else{
			Color tempColor = transform.GetChild (0).GetChild(3).GetComponent<SpriteRenderer> ().color;
			tempColor.a = 0;
			transform.GetChild (0).GetChild(3).GetComponent<SpriteRenderer> ().color = tempColor;
		}
	}
}
