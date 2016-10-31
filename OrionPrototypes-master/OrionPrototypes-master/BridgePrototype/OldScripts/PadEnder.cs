using UnityEngine;
using System.Collections;

public class PadEnder : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Pad") {
			col.transform.position= transform.parent.transform.position;
		}
	}
}
