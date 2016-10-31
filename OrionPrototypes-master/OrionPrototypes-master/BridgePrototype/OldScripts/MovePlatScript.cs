using UnityEngine;
using System.Collections;

public class MovePl : MonoBehaviour {

	float start;
	float end;
	bool up;

	// Use this for initialization
	void Start () {
		start = transform.position.y;
		end = transform.position.y + 5;
	}
	
	// Update is called once per frame
	void Update () {
		if(up){
			transform.position+= new Vector3(0,.05f,0);
			if(transform.position.y >end){
				up = !up;
			}
		}else{
			transform.position-= new Vector3(0,.05f,0);
			if(transform.position.y >end){
				up = !up;
			}
		}
	}
}
