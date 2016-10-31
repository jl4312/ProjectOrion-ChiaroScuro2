using UnityEngine;
using System.Collections;

public class LargeRock : MonoBehaviour {
	

	private bool player1;
	private bool player2;

	public bool PlayerOne
	{
		set { player1 = value; }
		get { return player1; }
	}

	public bool PlayerTwo
	{
		set { player2 = value; }
		get { return player2; }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if (player1 && player2)
		{
			rock = Instantiate(largeRockPrefab);
			rock.transform.position= p1.transform.position+ (p1.transform.forward)+(p1.transform.up*1.5f);
			rock.transform.parent = p1.transform;
			hasRock=true;
			Destroy(this);
		}*/
	}
}
