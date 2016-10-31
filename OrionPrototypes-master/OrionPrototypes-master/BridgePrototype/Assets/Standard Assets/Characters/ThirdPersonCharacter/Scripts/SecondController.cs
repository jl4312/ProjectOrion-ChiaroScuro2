using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondController : MonoBehaviour
{
	public GameObject playerPrefab;
	public GameObject camera;
	public GameObject player1;
	public GameObject player2;
	float speed = 3;

	void Update()
	{
		if (Input.GetKey (KeyCode.Q))
		{
			player1.transform.Rotate(new Vector3(0, -1, 0));
		}
		if (Input.GetKey (KeyCode.E))
		{
			player1.transform.Rotate(new Vector3(0, 1, 0));
		}
		if (Input.GetKey (KeyCode.A))
			player1.transform.Translate (Vector3.left * speed * Time.deltaTime);
		if (Input.GetKey(KeyCode.D))
			player1.transform.Translate (Vector3.right * speed * Time.deltaTime);
		if (Input.GetKey(KeyCode.W))
			player1.transform.Translate (Vector3.forward * speed * Time.deltaTime);
		if (Input.GetKey (KeyCode.S))
			player1.transform.Translate (Vector3.back * speed * Time.deltaTime);
		else 
			player2.transform.Translate (Vector3.zero);


		if (Input.GetKey (KeyCode.Keypad7))
		{
			player2.transform.Rotate(new Vector3(0, -1, 0));
		}
		if (Input.GetKey (KeyCode.Keypad9))
		{
			player2.transform.Rotate(new Vector3(0, 1, 0));
		}
		if (Input.GetKey(KeyCode.Keypad4))
			player2.transform.Translate (Vector3.left * speed * Time.deltaTime);
		if (Input.GetKey(KeyCode.Keypad6))
			player2.transform.Translate (Vector3.right * speed * Time.deltaTime);
		if (Input.GetKey(KeyCode.Keypad8))
			player2.transform.Translate (Vector3.forward * speed * Time.deltaTime);
		if (Input.GetKey(KeyCode.Keypad5))
			player2.transform.Translate (Vector3.back * speed * Time.deltaTime);
		else 
			player2.transform.Translate (Vector3.zero);

	}
}