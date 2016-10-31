using UnityEngine;
using System.Collections;

public class DamScript : MonoBehaviour {
	public bool goingUp;
	private float fullDown;
	private float currentDown;
	
	// Use this for initialization
	void Start () {
		goingUp = false;
		currentDown = 0;
		fullDown = -25;
	}
	// Update is called once per frame
	void Update () {
		if (goingUp) {
			if (currentDown<0) {
				currentDown += .1f;
				transform.position =  transform.position + new Vector3 (0, .1f ,0);
			}
		} else {
			if (currentDown>fullDown) {
				currentDown -= .1f;
				transform.position =  transform.position + new Vector3 (0,- .1f,0);
			}
		}
		goingUp = true;
	}
}
