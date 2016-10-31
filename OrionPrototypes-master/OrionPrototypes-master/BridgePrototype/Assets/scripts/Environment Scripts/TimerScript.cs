using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour {
	
	// denotes if the timer is running
	public bool active;
	
	// the values for the overall timer amount, the time when the timer began, and the current value of the timer
	public float TimerAmount;
	private float timerStart;
	private float timerCurrent;
	
	// reference to object that is to be changed color at the end of the timer;
	public GameObject target;
	
	// Use this for initialization
	void Start () {
		timerStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (active)
		{
			// running the timer if active and changing the color when we hit the timer (then resetting again)
			timerCurrent = Time.time - timerStart;
			
			if (timerCurrent > TimerAmount)
			{
				timerStart = Time.time;
				target.GetComponent<ColorScript>().isWhite = !target.GetComponent<ColorScript>().isWhite;
			}
			
		}
		
	}
}

