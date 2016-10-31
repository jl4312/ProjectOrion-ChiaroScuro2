using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
	
	public Vector3 velo;
	public int ballNum;
	
	
	// Update is called once per frame
	void Update()
	{
		// move the ball by its velocity and increment its shooter's reference to it's distance moved
		transform.position += velo;
		this.transform.GetComponentInParent<RockShooterScript>().distGone [ballNum] += velo.magnitude;
	}
	
	void OnCollisionEnter(Collision col)
	{
		// if the ball hits objects specifically designed to stop them then we destroy the ball
		if (col.gameObject.tag == "Screw" || col.gameObject.tag == "Protected" || col.gameObject.tag == "Platform")
		{
			transform.parent.GetComponent<RockShooterScript>().DestroyBall(ballNum);
		}
	}
	
	// this function calls a corresponding function in it's parent shooter to destroy it entirely
	public void DestroyBall()
	{
		transform.parent.GetComponent<RockShooterScript>().DestroyBall(ballNum);
	}
}
