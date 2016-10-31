using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if((Input.GetButtonDown("P1X") || Input.GetButtonDown("P1S") || Input.GetButtonDown("P1T") || Input.GetButtonDown("P1O") || Input.GetButtonDown("P2X") || Input.GetButtonDown("P2S") || Input.GetButtonDown("P2T") || Input.GetButtonDown("P2O") || Input.GetButtonDown("P2Share") || Input.GetButtonDown("P1Share")))
        {
            Application.Quit();
        }
	}
}
