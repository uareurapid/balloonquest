using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour {


	public float startDelay = 2.0f;
	public float switchInterval = 2.0f;//note, if there is a swap script these values must match those

	public bool isLaserController = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Switch() {
		if (isLaserController) {
			Invoke("LaserSwitch",0.5f);//half second later
		}
	}

	void LaserSwitch() {
		MyBasicLaser laser = GetComponentInChildren<MyBasicLaser>();
		if(laser!=null && !laser.isAutomatic) {
			bool on = laser.LaserOn;
			if(on) {
				laser.DisableLaser();
			}
			else {
				laser.EnableLaser();
			}
		}
	}
}
