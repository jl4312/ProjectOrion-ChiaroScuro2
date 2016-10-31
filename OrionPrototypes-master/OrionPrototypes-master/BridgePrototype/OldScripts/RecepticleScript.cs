using UnityEngine;
using System.Collections;

public class RecepticleScript : MonoBehaviour {

	public float cost = 1;
	public float paid = 0;
	public GameObject Target;

	// Use this for initialization
	void SendSignal () {
		Target.GetComponent<RecTargetScript>().activated=true;
	}
	
	// Update is called once per frame
	void Update () {
		if(paid>=cost){
			SendSignal();
		}
	}
}
