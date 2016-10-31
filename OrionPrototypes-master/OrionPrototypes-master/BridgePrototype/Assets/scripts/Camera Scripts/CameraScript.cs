using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	
	// references to both players
	public GameObject player1;
	public GameObject player2;
	
	// designer-set minimum and maximum distances the camera can be from players
	public float minDistance;
	public float maxDistance;
	
    //Music
    //The Nymphaeum (Part III) (2:53) by Angelwing
    //Under Creative Commons
    //http://www.freemusicpublicdomain.com/royalty-free-new-age-music/
    private AudioSource soundSource;

	// median point between player locations
	private Vector3 median;
	
	//distance between players
	private float playerDist;
	
	// the next PPoint to lerp to after the current lerp is over
	private GameObject queuedPPoint;
	
	// the next lerp time to lerp by after the current lerp is over
	private float queuedLerpT;
	
	// ray and hit data used to check is players are being obstructed
	private Ray ray;
	private RaycastHit hit;
	
	// the distance the camera is from the players at any given time
	private float camDistance;
	
	// denotes whether the camera is currently in a lerp between perspective points
	private bool lerping;
	
	// references to the previous PPoint and current PPoint (used for lerping)
	private GameObject previousPPoint;
	private GameObject currentPPoint;
	
	// the timer data used for the lerp
	private float timer;
	private float timerStart;
	private float lerpTime;
	
	// denotes whether player 1 (obstructed1) is being obstructed or if player2 (obstructed2) is beign obestructed from camera
	private bool obstructed1;
	private bool obstructed2;
	
	// Use this for initialization
	void Start()
	{

        soundSource = GetComponent<AudioSource>();
        soundSource.Play();

		// set initial player distance and median
		playerDist = (player1.transform.position - player2.transform.position).magnitude;
		median = player1.transform.position + ((player1.transform.position - player2.transform.position).normalized * playerDist / 2);
		
		// setting the max and min distance to default values if designers have not set them
		if (maxDistance == 0)
		{
			maxDistance = 100;
		}
		if (minDistance == 0)
		{
			minDistance = 3;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
        //Make sure music stays
        if (!soundSource.isPlaying)
            soundSource.Play();

		// initial check to be sure we have a Perspective point to give us a direction and distance from player
		if (currentPPoint)
		{

			// updating player distance
			playerDist = (player1.transform.position - player2.transform.position).magnitude;
			
			// calculating camera distance using the perspective point distance and then the player distance to move in and out with players
			camDistance = currentPPoint.GetComponent<PPointScript>().distance * playerDist;
			
			// clamping the camera distance, the use of offset is so perspective points can denote a specific minimum distance just for themselves
			camDistance = Mathf.Max(camDistance, minDistance + currentPPoint.GetComponent<PPointScript>().offset);
			
			camDistance = Mathf.Min(camDistance, maxDistance);
			
			// calculating player median
			median = player2.transform.position + ((player1.transform.position - player2.transform.position).normalized * playerDist / 2);
			
			// using the current perspective point as a stand-in for the camera to do obstruction checks
			currentPPoint.transform.position = median - (currentPPoint.transform.forward * camDistance);
			
			this.transform.position = currentPPoint.transform.position;
			
			obstructed1 = false;
			obstructed2 = false;
			
			//Doing raycast checks to be sure we can see the players
			ray = new Ray(player1.transform.position, this.transform.position - player1.transform.position);
			if (Physics.Raycast(ray, out hit, (this.transform.position - player1.transform.position).magnitude))
			{
				if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject.tag != "Player" && !hit.collider.isTrigger && hit.collider.gameObject.tag != "Othe" && !hit.collider.isTrigger && hit.collider.gameObject.tag != "Screw" && hit.collider.gameObject.tag != "Invis" && hit.collider.gameObject.tag != "Mushroom")
				{
					obstructed1 = true;
				}
			}
			
			ray = new Ray(player2.transform.position, this.transform.position - player2.transform.position);
			if (Physics.Raycast(ray, out hit, (this.transform.position - player2.transform.position).magnitude))
			{
				if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject.tag != "Player" && !hit.collider.isTrigger && hit.collider.gameObject.tag != "Othe" && !hit.collider.isTrigger && hit.collider.gameObject.tag != "Screw" && hit.collider.gameObject.tag != "Invis" && hit.collider.gameObject.tag != "Mushroom")
				{
					obstructed2 = true;
				}
			}
			
			// if both players are obstructed we want to pull the camera in so it is in front of the obstruction
			if (obstructed1 && obstructed2)
			{
				// raycast from the median and move the camera to the obstruction
				ray = new Ray(median, this.transform.position - median);
				Physics.Raycast(ray, out hit, (this.transform.position - median).magnitude);
				this.transform.position = median + (this.transform.position - median).normalized * (hit.distance);
			}
			
			// lerping check and corresponding lerp code
			if (lerping)
			{
				timer = Time.time - timerStart;
				
				// so long as we haven't gone through the full time period of the lerp we continue to do so
				if (timer > lerpTime)
				{
					lerping = false;
					transform.position = currentPPoint.transform.position;
					if (queuedPPoint)
					{
						TransitionTo(queuedPPoint, queuedLerpT);
						queuedPPoint = null;
					}
				} else
				{
					transform.position = Vector3.Slerp(previousPPoint.transform.position, currentPPoint.transform.position, (timer / lerpTime));
					transform.forward = Vector3.Slerp(previousPPoint.transform.forward, currentPPoint.transform.forward, (timer / lerpTime));
				}
				
			}
		}
		
	}
	
	// function called to give the camera the information needed to begin a lerp to the next Perspective point
	public void TransitionTo(GameObject newPPoint, float lerpT)
	{
		// make sure we aren't already in the middle of a transition (thus, a lerp)
		if (!lerping)
		{
			// if we don't have a current perspective point or the new one is the same as current then we automatically set the camera without lerp
			if (!currentPPoint || currentPPoint.name == newPPoint.name)
			{
				transform.position = newPPoint.transform.position;
				transform.forward = newPPoint.transform.forward;
				currentPPoint = newPPoint;
			} 
			// otherwise we setup our variables to prepare for the transitional lerp
			else
			{
				lerpTime = lerpT;
				
				previousPPoint = currentPPoint;
				
				currentPPoint = newPPoint;
				
				timerStart = Time.time;
				
				lerping = true;
			}
		}
		// if camera is already lerping then we queue a perspective point to lerp to directly after, only one can be queued at a time
		else
		{
			queuedPPoint = newPPoint;
			queuedLerpT = lerpT;
		}
	}
}
