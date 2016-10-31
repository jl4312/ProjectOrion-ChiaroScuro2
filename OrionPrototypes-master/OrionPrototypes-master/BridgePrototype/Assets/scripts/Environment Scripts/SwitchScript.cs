using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {
	
	public bool justEntered;
	public bool OnColorOffMovement;
	private bool active;
	public GameObject player1;
	public GameObject myCamera;
	public GameObject target;
	
	// Use this for initialization
	void Start () {
		myCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (justEntered == true) {
			justEntered = false;
		}else {
			if (player1 && Input.GetButtonDown (player1.GetComponent<PlayerScript> ().mySButton)) {
				player1.transform.position = transform.position - transform.forward;
				player1.transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				player1.transform.GetChild (0).GetComponent<Renderer> ().enabled = true;
				player1.transform.GetComponent<Collider> ().enabled = true;
				
				if(myCamera.GetComponent<CameraScript> ().player1 == this.gameObject){
					myCamera.GetComponent<CameraScript> ().player1 = player1;
				}else {
					myCamera.GetComponent<CameraScript> ().player2 = player1;
				}
				
				player1 =null;
			}
		}
		
		if (player1 && Input.GetButtonDown(player1.GetComponent<PlayerScript>().myXButton))
		{
			active = !active;
			
			if(OnColorOffMovement){
				target.GetComponent<ColorScript>().isWhite = !target.GetComponent<ColorScript>().isWhite;
			}else{
				target.GetComponent<TriggerScript>().triggered = !target.GetComponent<TriggerScript>().triggered;
			}
		}
		
		if (!active)
		{
			this.gameObject.GetComponent<Renderer>().material.color = Color.red;
		}
		else
		{
			this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
		}
	}
}
