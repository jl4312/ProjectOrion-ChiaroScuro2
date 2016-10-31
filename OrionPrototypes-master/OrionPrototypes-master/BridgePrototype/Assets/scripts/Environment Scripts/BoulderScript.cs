using UnityEngine;
using System.Collections;

public class BoulderScript : MonoBehaviour
{
	
	// the trigger that will destroy the boulder on collision
	public GameObject trigger;
	
	void OnCollisionEnter(Collision col)
	{
		// the boulder need only check if it is hit by its trigger, and then it is essentially destroyed
		if (col.gameObject == trigger)
		{
			Destroy(trigger);
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<Collider>().enabled = false;
		}
	}
}
