using UnityEngine;
using System.Collections;

public class PPointScript : MonoBehaviour {
	// The float representing the magnitude of the camera's "arm"
	public float distance;
	
	// the offset serves as a sort of perspective point-specific minimum distance
	public float offset;
	
	// Use this for initialization
	void Start ()
	{
		// set the distance for designers if they haven't already
		if(distance==0){
			distance=1;
		}
	}
}

