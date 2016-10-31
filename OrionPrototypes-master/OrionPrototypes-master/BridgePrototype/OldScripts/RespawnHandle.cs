using UnityEngine;
using System.Collections;

public class RespawnHandle : MonoBehaviour {

	public GameObject player1Respawn;
	public GameObject player2Respawn;

	private bool play1, play2;

	public bool Player1Check
	{
		set { play1 = value; }
		get { return play1; }
	}

	public bool Player2Check
	{
		set { play2 = value; }
		get { return play2; }
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name == "Player 1")
		{
			play1 = true;
		}
		else if (col.gameObject.name == "Player 2")
		{
			play2 = true;
		}

		if (play1 && play2)
		{
			GameObject.Find("Player 1").GetComponent<Player1Script>().RespawnPosition = player1Respawn.transform.position;
			GameObject.Find("Player 2").GetComponent<Player1Script>().RespawnPosition = player2Respawn.transform.position;
			Destroy(transform.gameObject);
		}
	}
}
