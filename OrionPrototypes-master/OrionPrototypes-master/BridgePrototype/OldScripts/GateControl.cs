using UnityEngine;
using System.Collections;

public class GateControl : MonoBehaviour {

	public GameObject Gate;

	public void CloseGate(){
		Destroy(Gate);
	}

	public void ShrinkGate(){
		Gate.gameObject.transform.localScale -= Vector3.one * Time.deltaTime;
	}

	public float cost = 1;
	public float paid = 0;
}
