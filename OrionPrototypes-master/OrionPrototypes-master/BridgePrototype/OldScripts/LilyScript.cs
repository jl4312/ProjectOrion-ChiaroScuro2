using UnityEngine;
using System.Collections;

public class LilyScript : MonoBehaviour {

	public Vector3 movementVec;

	private float originalHeight;

	private float timeElapsed;

	// Use this for initialization
	void Start () {
		//Setting the initial elapsed time
		timeElapsed = 0.0f;
		originalHeight = transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		timeElapsed += Time.deltaTime;
		transform.position += (movementVec * Time.deltaTime);
		if ((transform.parent.position- transform.position).magnitude+3 >GetComponentInParent<PadSpawn>().Distance) {
			transform.position = transform.position - new Vector3(0,.01f,0);
		}
		if ((transform.parent.position- transform.position).magnitude >GetComponentInParent<PadSpawn>().Distance) {
			transform.position =new Vector3(transform.position.x,originalHeight,transform.position.z+GetComponentInParent<PadSpawn>().Distance);
			GetComponent<Renderer> ().enabled = true;
			GetComponent<BoxCollider> ().enabled = true;
			for (int i=0;i<gameObject.transform.GetChildCount();i++) {
				if(gameObject.transform.GetChild(i).tag=="Player"){
					gameObject.transform.GetChild(i).GetComponent<Player1Script>().Respawn("");
				}
			}
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Dam") {
			this.GetComponent<Renderer> ().enabled = false;
			this.GetComponent<BoxCollider> ().enabled = false;
		}
	}
}
