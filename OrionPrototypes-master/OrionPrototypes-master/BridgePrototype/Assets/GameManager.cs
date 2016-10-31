using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    string myExitButton = "P1Exit";
    string myExitButton2 = "P2Exit";

	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKey(KeyCode.Escape) || Input.GetButtonDown(myExitButton) || Input.GetButtonDown(myExitButton2))
        {
            Application.Quit();
        }
	}
}
