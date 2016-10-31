using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
    private GameObject P1;
    private GameObject P2;

	// Use this for initialization
	void Start () {

        //P1 = this.gameObject.transform.GetChild(0).gameObject;
        //P2 = this.gameObject.transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    if((Input.GetButtonDown("P1X") || Input.GetButtonDown("P1S") || Input.GetButtonDown("P1T") || Input.GetButtonDown("P1O") || Input.GetButtonDown("P2X") || Input.GetButtonDown("P2S") || Input.GetButtonDown("P2T") || Input.GetButtonDown("P2O")))
        {
            Application.LoadLevel(1);
            Debug.Log("Loading scene!");
        }
	}
}
