using UnityEngine;
using System.Collections;

public class PushMe : MonoBehaviour {

	bool p1, p2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject play1 = Camera.allCameras[0].GetComponent<CameraScript>().player1.gameObject;
		GameObject play2 = Camera.allCameras[0].GetComponent<CameraScript>().player2.gameObject;

		if (p1 && p2)
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().mass = .1f;
			GetComponent<Rigidbody>().drag = .1f;
			GetComponent<Rigidbody>().AddForce((play1.GetComponent<Rigidbody>().velocity) + (play2.GetComponent<Rigidbody>().velocity));
		}
		else
		{
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponent<Rigidbody>().mass = 10;
			GetComponent<Rigidbody>().drag = 60;
		}
	}

	void OnCollisionStay(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			p1 = true;
		}

		if (col.gameObject.tag == "Player2")
		{
			p2 = true;
		}
	}

	void OnCollisionExit(Collision col)
	{
		p1 = false;
		p2 = false;
	}
}
