using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PadSpawn : MonoBehaviour {
	
	public float Distance;
	private float timeElapsed;
	public Vector3 MovementVec;

	
	// Use this for initialization
	void Start () {
		//Setting the initial elapsed time
		timeElapsed = 0.0f;
		for (int i=0;i<gameObject.transform.GetChildCount();i++) {
			if(gameObject.transform.GetChild(i).tag=="Pad"){
				gameObject.transform.GetChild(i).GetComponent<LilyScript>().movementVec = MovementVec;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
	}
}
