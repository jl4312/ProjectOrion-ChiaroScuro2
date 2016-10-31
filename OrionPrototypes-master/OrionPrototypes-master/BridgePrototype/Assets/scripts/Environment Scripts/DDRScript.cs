using UnityEngine;
using System.Collections;

public class DDRScript : MonoBehaviour {

    public GameObject cannon1;
    public GameObject cannon2;
    public GameObject cannon3;
    public GameObject cannon4;
    public float timeBetweenShots;
    public float shotSpeed;

    private float timer;
    private float timerStart;

	// Use this for initialization
	void Start () {
        cannon1.GetComponent<RockShooterScript>().rockSpeed = shotSpeed;
        cannon2.GetComponent<RockShooterScript>().rockSpeed = shotSpeed;
        cannon3.GetComponent<RockShooterScript>().rockSpeed = shotSpeed;
        cannon4.GetComponent<RockShooterScript>().rockSpeed = shotSpeed;

        /*cannon1.GetComponent<RockShooterScript>().DDRMode = true;
        cannon2.GetComponent<RockShooterScript>().DDRMode = true;
        cannon3.GetComponent<RockShooterScript>().DDRMode = true;
        cannon4.GetComponent<RockShooterScript>().DDRMode = true;*/
        timerStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        timer = Time.time - timerStart;

        if (timer > timeBetweenShots)
        {

            int num1 = Random.Range(0, 4);

            int num2 = num1;

            while (num1 == num2)
            {
                num2 = Random.Range(0, 4);
            }

            switch (num1)
            {
                case 0:
                    cannon1.GetComponent<RockShooterScript>().Fire();
                    break;

                case 1:
                    cannon2.GetComponent<RockShooterScript>().Fire();
                    break;

                case 2:
                    cannon3.GetComponent<RockShooterScript>().Fire();
                    break;

                case 3:
                    cannon4.GetComponent<RockShooterScript>().Fire();
                    break;
            }

            switch (num2)
            {
                case 0:
                    cannon1.GetComponent<RockShooterScript>().Fire();
                    break;

                case 1:
                    cannon2.GetComponent<RockShooterScript>().Fire();
                    break;

                case 2:
                    cannon3.GetComponent<RockShooterScript>().Fire();
                    break;

                case 3:
                    cannon4.GetComponent<RockShooterScript>().Fire();
                    break;
            }

            timerStart = Time.time;
        }
	}
}
