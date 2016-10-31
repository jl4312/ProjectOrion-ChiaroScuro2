using UnityEngine;
using System.Collections;

public class RespawnZoneScript : MonoBehaviour {
	
	// the bools tracking if both players have entered the zone (needs both to update the respawn locations)
	private bool hasP1;
	private bool hasP2;
	
	// the spawn locations for both players to update their respawn locations to when they hit the trigger
	private Vector3 P1S;
	private Vector3 P2S;
	
	// reference to the scene camera
	private GameObject cam;
	
	// Use this for initialization
	void Start () {
		// setting the camera and the two spawn positions using children (set by designers)
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		P1S = transform.GetChild (0).position;
		P2S = transform.GetChild (1).position;
	}
	
	
	public void OnTriggerEnter(Collider col)
	{
		// if both players pass through the trigger (in player or vehicle form) then we update the rspawn locations
		if (col.tag == "Player" || (col.tag == "SingleControlRock" && col.GetComponent<SingleControlRock> ().player1)) {
			if(col.gameObject == cam.GetComponent<CameraScript> ().player1){
				hasP1 = true;
			} else {
				hasP2 = true;
			}
			if(hasP1 && hasP2){
				cam.GetComponent<CameraScript> ().player1.GetComponent<PlayerScript>().respawnPosition = P1S;
				cam.GetComponent<CameraScript> ().player2.GetComponent<PlayerScript>().respawnPosition = P2S;
			}
		}
	}
}