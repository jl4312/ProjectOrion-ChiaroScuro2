using UnityEngine;
using System.Collections;

public class StartMenuText : MonoBehaviour {
    public bool Player1;
    public string myXButton;
    public bool ready;

    private TextMesh myText;
    

	// Use this for initialization
	void Start () {
        if (Player1)
        {
            myXButton = "P1X";
        }
        else
        {
            myXButton = "P2X";
        }
        myText = GetComponent<TextMesh>();
        ready = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton(myXButton))
        {
            if (Player1)
                myText.text = "Player 1 Ready";
            else
                myText.text = "Player 2 Ready";
            ready = true;
        }
	}
}
