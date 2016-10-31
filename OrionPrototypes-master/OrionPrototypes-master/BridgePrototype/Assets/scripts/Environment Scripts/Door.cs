using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
	
	// the location the door moves to to open
	public GameObject endPoint;
	
	// the reference to the door model and collider (this is not attached to the main door as otherwise the end point would move with the door)
	public GameObject actualDoor;
	
	// the location at which the door begins
	private Vector3 startPoint;
	
	// the vector of movement for opening
	Vector3 moveDir;
	
	// Use this for initialization
	void Start()
	{
		// setting an end point if user does not
		if (!endPoint)
		{
			endPoint = new GameObject();
			endPoint.transform.position = this.transform.position + new Vector3(0, -1, 0);
		}
		// setting up the movement vector
		moveDir = (endPoint.transform.position - transform.position).normalized;
		startPoint = this.transform.position;
		// adding a trigger component as that will be nessecary to have the door triggered to open
		if (!this.GetComponent<TriggerScript>())
		{
			this.gameObject.AddComponent<TriggerScript>();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		// if the trigger component is triggered then we move up until we are at the door's max point (we use dot product as to allow for arbitrary orientation)
		if (this.GetComponent<TriggerScript>().triggered)
		{
			if (Vector3.Dot(endPoint.transform.position, moveDir) > Vector3.Dot(actualDoor.transform.position, moveDir))
			{
				actualDoor.transform.position += moveDir * .04f;
			}
		}
		// otherwise we move down until we hit the start point
		else
		{
			if (Vector3.Dot(startPoint, moveDir) < Vector3.Dot(actualDoor.transform.position, moveDir))
			{
				actualDoor.transform.position -= moveDir * .04f;
			}
		}
	}
}
