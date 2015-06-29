using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour {

	private bool isVisible = false;
	private MovingPlatformScript platformScript;
	// Use this for initialization
	void Start () {
	
	   platformScript = GetComponent<MovingPlatformScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnBecameVisible() {
		
		//disable movement when collider becames visible
		isVisible = true;
		if(platformScript!=null) {
		  Invoke("StopMovement",1.0f);
		}
	}
	
	void StopMovement() {
		platformScript.enabled = false;
	}
	
	void OnBecameInvisible() {
	
	}
}
