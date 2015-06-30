using UnityEngine;
using System.Collections;

public class LevelChangeListener : MonoBehaviour {

	public int level = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible() {
		Debug.Log ("ON VISIBLE: " + level);
		Camera.main.GetComponent<CameraZoomInOutScript> ().SwipeToLevelEnded (level);
	}

	void OnBecameInvisible() {
		Debug.Log ("ON INVISIBLE: " + level);
	}
}
