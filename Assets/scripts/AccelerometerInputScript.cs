using UnityEngine;
using System.Collections;

//cool stuff here too: http://forum.unity3d.com/threads/understanding-the-gyroscope.205267/
public class AccelerometerInputScript : MonoBehaviour {

    PlayerScript player;
	bool supportsAccelerometer= false;
	// Use this for initialization
	void Start () {
	  supportsAccelerometer = SystemInfo.supportsAccelerometer;
	  if(supportsAccelerometer) {
			GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
	  		if(playerObj) {
	    		player = playerObj.GetComponent<PlayerScript>();
	  		}
	  }

	}
	
	// Update is called once per frame
	void Update () {

	  if(supportsAccelerometer && player!=null) {
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
