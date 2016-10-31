using UnityEngine;
using System.Collections;

public class PPointTrigger : MonoBehaviour
{
	
	// the perspective point this trigger is tied to
	public GameObject pPoint;
	
	// denoted whether both players have to hit the trigger for the camera to transition to the next perspective
	public bool needsTwoPlayers;
	
	// booleans tracking which players have hit the zone
	private bool hasP1;
	private bool hasP2;
	
	// the time it should take to transition from the current camera orientation to the one denoted by this trigger's perspective point
	private float lerpTime;
	private GameObject camera;
	
	// Use this for initialization
	void Start()
	{
		// setting the camera to the one in the scene
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		
		// setting the lerptime ourselves if the designer does not
		if (lerpTime == 0)
		{
			lerpTime = 1;
		}
	}
	
	public void OnTriggerEnter(Collider col)
	{
		// if it colliders with a player or a block beign controlled by one then we either begin a transition or indicate that one of two players have entered
		if (col.tag == "Player" || (col.tag == "SingleControlRock" && col.GetComponent<SingleControlRock>().player1))
		{
			if (needsTwoPlayers)
			{
				if (col.gameObject == camera.GetComponent<CameraScript>().player1)
				{
					hasP1 = true;
				} else
				{
					hasP2 = true;
				}
				// if both have entered for a zone needing two then we transition
				if (hasP1 && hasP2)
				{
					camera.GetComponent<CameraScript>().TransitionTo(pPoint, lerpTime);
				}
			} else
			{
				camera.GetComponent<CameraScript>().TransitionTo(pPoint, lerpTime);
			}
		}
	}
}
