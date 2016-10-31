using UnityEngine;
using System.Collections;

public class MushroomScripts : MonoBehaviour {
	
	// represents the force added to the player when they bounce
	public float bounceForce;
	
	// bool used to toggle whether the bounce takes into account previous velocity
	public bool additive;
	
	void OnTriggerEnter(Collider col){
		// bounces players and control rocks (which are basically player vehicles) if they collider with its bounce zone
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "SingleControlRock") {
			if (additive) {
				col.GetComponent<Rigidbody> ().velocity = new Vector3 (col.GetComponent<Rigidbody> ().velocity.x, 0, col.GetComponent<Rigidbody> ().velocity.z);
				col.GetComponent<Rigidbody> ().velocity += transform.up * bounceForce;
				;
			} else {
				col.GetComponent<Rigidbody> ().velocity = transform.up * bounceForce;
			}
		}
		
		// if a harmful projectile hits the mushroom, it just redirects in the direction the mushroom is aimed
		if (col.gameObject.tag == "Harmful") {
			float speed = col.GetComponent<BallScript>().velo.magnitude;
			col.GetComponent<BallScript>().velo = transform.up;
			col.GetComponent<BallScript>().velo = col.GetComponent<BallScript>().velo.normalized * speed;
		}
	}
}
