using UnityEngine;
using System.Collections;

public class MovePlat : MonoBehaviour
{
	
	// the array of points that the moving platform moves along
	public Vector3[] Points;
	
	// the reference to the platform model and collider being moved
	public GameObject actualPlat;
	
	// denotes whether the platform moves backwards thruogh the point path after it reaches the end
	public bool reverses;
	
	// used to denote the speed of the platform
	public float speed;
	
	// denotes whether the platform is currently moving forward or backward on the path of points
	private bool backwards;
	
	// the current index we are at for the array of points of the path
	private int pathIndex;
	
	// the original starting point of the platform
	private Vector3 startPoint;
	Vector3 moveDir;
	
	// Use this for initialization
	void Start()
	{
		// setting speed if user has not
		if (speed == 0)
		{
			speed = 1;
		}
		// setting initial path index
		pathIndex = 0;
		
		// if this object has more than one child (the first being the model itself) then we can create the array with the size of points, which are children
		if (transform.childCount > 1)
		{
			Points = new Vector3[transform.childCount];
		}
		
		// setting up the array using the objects children (assumed to be points)
		for (int i=0; i<transform.childCount; i++)
		{
			if (i == 0)
			{
				Points [i] = startPoint;
			}
			Points [i] = transform.GetChild(i).transform.position;
		}
		
		// setting up the move direction and start point
		moveDir = (Points [0] - actualPlat.transform.position).normalized;
		startPoint = this.transform.position;
		
		// this needs a trigger so if we don't have one we make one
		if (!this.GetComponent<TriggerScript>())
		{
			this.gameObject.AddComponent<TriggerScript>();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		// if the platform is triggered to move we go through the movement process
		if (this.GetComponent<TriggerScript>().triggered)
		{
			// we use dot product to make sure the platform has not reached its next point
			if (Vector3.Dot(Points [pathIndex], moveDir) > Vector3.Dot(actualPlat.transform.position, moveDir))
			{
				actualPlat.transform.position += moveDir * .04f * speed;
			} 
			// if ti has we need to increment to the next path, unless we get to the end, in which case we reverse the direction 
			else
			{
				if (pathIndex < Points.Length - 1 && !backwards || pathIndex > 0 && backwards)
				{
					if (!backwards)
					{
						pathIndex++;
					} else
					{
						pathIndex--;
					}
					moveDir = (Points [pathIndex] - actualPlat.transform.position).normalized;
				} else if (reverses)
				{
					backwards = !backwards;
				}
			}
			
		}
	}
}
