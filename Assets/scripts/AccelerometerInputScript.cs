using UnityEngine;
using System.Collections;
using BalloonQuest;

//cool stuff here too: http://forum.unity3d.com/threads/understanding-the-gyroscope.205267/
public class AccelerometerInputScript : MonoBehaviour {

    PlayerScript player;
	bool supportsAccelerometer= false;
	bool accelerometerActivated = false;
	// Use this for initialization
	void Start () {
	  //has support?
	  supportsAccelerometer = SystemInfo.supportsAccelerometer;
	  if(supportsAccelerometer && IsAccelerometerActivated()) {
			GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
	  		if(playerObj) {
	    		player = playerObj.GetComponent<PlayerScript>();
				//disable normal swipe
				player.GetComponent<SwipeScript>().enabled = false;
	  		}
	  }

	}

	bool IsAccelerometerActivated() {
		int useAccelerometer = PlayerPrefs.GetInt (GameConstants.ACCELEROMETER_SETTINGS_KEY, 0);
		accelerometerActivated = (useAccelerometer == 1) ? true : false;
		return accelerometerActivated;
	}
	// Update is called once per frame
	void Update () {

	  if(supportsAccelerometer && accelerometerActivated && player!=null) {
		 float x = Input.acceleration.x;
		 if(x<0f) {
		   player.MoveBackward();
		 }
		 else if(x>0f) {
		   player.MoveForward();
		 }
		 else {
		   player.PlayerStationary();
		 }
		//transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);
	  }
	    
	}
}
