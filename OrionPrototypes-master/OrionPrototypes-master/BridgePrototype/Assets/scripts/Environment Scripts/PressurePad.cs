using UnityEngine;
using System.Collections;

public class PressurePad : MonoBehaviour {
	
	// reference to an object that triggers the pressure pad
	public GameObject trigger;
	
	// reference to the object that is triggered when the perspective point is
	public GameObject target;
	
	void OnTriggerEnter(Collider col){
		// if the trigger enters the Pressure pad's area then we trigger our target
		if (col.gameObject == trigger) {
			transform.GetChild(0).transform.position -= new Vector3(0,.5f,0);
			target.GetComponent<TriggerScript>().triggered = true;
		}
	}
	
	void OnTriggerExit(Collider col){
		// if the tigger leaves the pressure pad's area then we un-trigger our target
		if (col.gameObject == trigger) {
			transform.GetChild(0).transform.position += new Vector3(0,.5f,0);
			target.GetComponent<TriggerScript>().triggered = false;
		}
	}
}
