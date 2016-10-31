using UnityEngine;
using System.Collections;

public class LavaReset : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			GameObject.Find("Player 1").SendMessage("Respawn", "come back");
			GameObject.Find("Player 2").SendMessage("Respawn", "come back");

			GameObject[] checkpoints;
			checkpoints = GameObject.FindGameObjectsWithTag("checkpoint");

			if (col.gameObject.name == "Player 1")
			{
				foreach ( GameObject g in checkpoints)
				{
					g.GetComponent<RespawnHandle>().Player1Check = false;
				}
			}
			else if (col.gameObject.name == "Player 2")
			{
				foreach ( GameObject g in checkpoints)
				{
					g.GetComponent<RespawnHandle>().Player2Check = false;
				}
			}

			GameObject[] movedPlatforms;
			movedPlatforms = GameObject.FindGameObjectsWithTag("Panel");

			foreach ( GameObject g in movedPlatforms)
			{
				g.SendMessage("ResetPosition", "go back");
			}
		}
	}
}
