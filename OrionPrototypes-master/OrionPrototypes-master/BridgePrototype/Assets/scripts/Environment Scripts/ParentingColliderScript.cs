using UnityEngine;
using System.Collections;

public class ParentingColliderScript : MonoBehaviour
{

	//This script can be used on triggers attached to objects to have players parented with them when in the trigger (good for platforms)

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			col.transform.parent = this.transform.parent;
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == "Player") {
			col.transform.parent = null;
		}
	}
}
