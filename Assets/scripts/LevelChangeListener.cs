using UnityEngine;
using System.Collections;

public class LevelChangeListener : MonoBehaviour {

	public int level = 1;
	public bool isVisible = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	  /*if(isVisible) {
	  
		Vector3 CameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 
		Camera.main.nearClipPlane));
	  		//check if we are on center, only then stop the movement of the camera
			if (Physics.Raycast(CameraCenter, transform.forward, 100))  {
			
			Debug.Log("in the center, stop right now");
				CameraZoomInOutScript cameraScript = Camera.main.GetComponent<CameraZoomInOutScript> ();
				cameraScript.SwipeToLevelEnded (level);	
			}
	  }*/
	}

	void OnBecameVisible() {
		Debug.Log ("ON VISIBLE: " + level );
		if(!isVisible) {
		
			isVisible = true;//cameraScript.SwipeToLevelEnded (level);
			//CameraZoomInOutScript cameraScript = Camera.main.GetComponent<CameraZoomInOutScript> ();
			//cameraScript.SwipeToLevelEnded (level);			
		}
		
		
	}

	void OnBecameInvisible() {
		Debug.Log ("ON INVISIBLE: " + level);
	}
}
